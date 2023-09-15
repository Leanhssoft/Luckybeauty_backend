using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Editions;
using BanHangBeautify.Entities;
using BanHangBeautify.MultiTenancy.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BanHangBeautify.MultiTenancy
{
    [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IRepository<HT_CongTy, Guid> _congTyRepository;
        private readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhRepository;
        public TenantAppService(
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IRepository<HT_CongTy, Guid> congTyRepository,
            IRepository<DM_ChiNhanh, Guid> chiNhanhRepository)
            : base(repository)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _congTyRepository = congTyRepository;
            _chiNhanhRepository = chiNhanhRepository;
        }
        [AbpAuthorize(PermissionNames.Pages_Tenants_Create)]
        public override async Task<TenantDto> CreateAsync(CreateTenantDto input)
        {
            string dbName = input.TenancyName;
            string dataSource = "DEV3\\SQL2018";
            string userId = "sa";
            string password = "sa@123";
            string connecStringInServer = $"data source={dataSource};initial catalog={dbName};persist security info=True;user id={userId};password={password};multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
            if (string.IsNullOrEmpty(input.ConnectionString))
            {
                input.ConnectionString = connecStringInServer;
            }
            input.IsActive = true;
            CheckCreatePermission();

            // Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            await _tenantManager.CreateAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

            // We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // Create tenant database
                _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);
                // Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                // Grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                // create chinhanh truoc khi tao user
                Guid idChiNhanh = await CreateCuaHangWithTenant(input.Name, tenant.Id);

                // Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, idChiNhanh);
                await _userManager.InitializeOptionsAsync(tenant.Id);

                //CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                CheckErrors(await _userManager.CreateAsync(adminUser, "123qwe"));
                await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                // Assign admin user to role!
                CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return MapToEntityDto(tenant);
        }
        [NonAction]
        public async Task<Guid> CreateCuaHangWithTenant(string tenCuaHang, int idTenant)
        {
            HT_CongTy data = new HT_CongTy();
            data.Id = Guid.NewGuid();
            data.TenCongTy = tenCuaHang;
            data.TenantId = idTenant;
            data.CreatorUserId = AbpSession.UserId;
            data.CreationTime = DateTime.Now;
            await _congTyRepository.InsertAsync(data);
            await _congTyRepository.InsertAsync(data);
            DM_ChiNhanh chiNhanh = new DM_ChiNhanh();
            chiNhanh.Id = Guid.NewGuid();
            chiNhanh.MaChiNhanh = "CN_01";
            chiNhanh.TenChiNhanh = tenCuaHang;
            chiNhanh.IdCongTy = data.Id;
            chiNhanh.CreationTime = DateTime.Now;
            chiNhanh.TenantId = idTenant;
            chiNhanh.CreatorUserId = AbpSession.UserId;
            await _chiNhanhRepository.InsertAsync(chiNhanh);
            return chiNhanh.Id;
        }

        [AbpAuthorize(PermissionNames.Pages_Tenants_Edit)]
        public async Task<TenantDto> GetTenantForEdit(EntityDto input)
        {
            var tenantEditDto = ObjectMapper.Map<TenantDto>(await _tenantManager.GetByIdAsync(input.Id));
            tenantEditDto.ConnectionString = SimpleStringCipher.Instance.Decrypt(tenantEditDto.ConnectionString);
            return tenantEditDto;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Tenants_Edit)]
        public async Task UpdateTenant(TenantDto input)
        {
            CheckDeletePermission();
            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            tenant.IsActive = input.IsActive;
            tenant.Name = input.Name;
            tenant.TenancyName = input.TenancyName;
            if (!string.IsNullOrEmpty(input.ConnectionString))
            {
                tenant.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            }
            await _tenantManager.UpdateAsync(tenant);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task DeleteTenant(int id)
        {
            CheckDeletePermission();
            var tenant = await _tenantManager.GetByIdAsync(id);
            await _tenantManager.DeleteAsync(tenant);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<int> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi sảy ra vui lòng thử lại sau!"
            };
            {
                var finds = await Repository.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
                if (finds != null && finds.Count > 0)
                {
                    Repository.RemoveRange(finds);
                    result.Status = "success";
                    result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                }
                return result;
            }
        }
        protected override IQueryable<Tenant> CreateFilteredQuery(PagedTenantResultRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override void MapToEntity(TenantDto updateInput, Tenant entity)
        {
            // Manually mapped since TenantDto contains non-editable properties too.
            entity.Name = updateInput.Name;
            entity.TenancyName = updateInput.TenancyName;
            entity.IsActive = updateInput.IsActive;
        }
        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            await _tenantManager.DeleteAsync(tenant);
        }
        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

