using Abp.Application.Features;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.UI;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Consts;
using BanHangBeautify.Editions;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Seed;
using BanHangBeautify.Features;
using BanHangBeautify.MultiTenancy.Dto;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BanHangBeautify.MultiTenancy
{
    [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int,
        PagedTenantResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IRepository<HT_CongTy, Guid> _congTyRepository;
        private readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhRepository;
        private readonly IRepository<Setting, long> _settingRepository;
        private readonly IRepository<FeatureSetting, long> _featureSettingRepository;
        private readonly IRepository<HT_NhatKyThaoTac, Guid> _nhatKyThaoTacRepository;
        private readonly AbpZeroDbMigrator _migrator;
        private readonly IConfiguration _configuration;
        private readonly ISeedDataAppService _seedDataEntities;
        public TenantAppService(
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IRepository<HT_CongTy, Guid> congTyRepository,
            IRepository<DM_ChiNhanh, Guid> chiNhanhRepository,
            IRepository<Setting, long> settingRepository,
            IRepository<FeatureSetting, long> featureSettingRepository,
            IRepository<HT_NhatKyThaoTac, Guid> nhatKyThaoTacRepository,
            IConfiguration configuration,
            ISeedDataAppService seedDataEntities,
            AbpZeroDbMigrator migration
            )
            : base(repository)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _congTyRepository = congTyRepository;
            _chiNhanhRepository = chiNhanhRepository;
            LocalizationSourceName = SPAConsts.LocalizationSourceName;
            _settingRepository = settingRepository;
            _featureSettingRepository = featureSettingRepository;
            _nhatKyThaoTacRepository = nhatKyThaoTacRepository;
            _migrator = migration;
            _configuration = configuration;
            _seedDataEntities = seedDataEntities;
        }
        [AbpAuthorize(PermissionNames.Pages_Tenants_Create)]
        public override async Task<TenantDto> CreateAsync(CreateTenantDto input)
        {
            string dbName = "SSOFT_"+  input.TenancyName;
            string dataSource = _configuration["SqlServer:DataSource"];
            string userId = _configuration["SqlServer:UserId"];
            string password = _configuration["SqlServer:Password"];
            string connecStringInServer = $"data source={dataSource};initial catalog={dbName};persist security info=True;user id={userId};password={password};multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
            if (string.IsNullOrEmpty(input.ConnectionString))
            {
                input.ConnectionString = connecStringInServer;
            }
            input.IsActive = true;
            CheckCreatePermission();

            // Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            if (tenant.IsTrial == true)
            {
                tenant.SubscriptionEndDate = DateTime.Now.AddDays(7);
            }
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            //var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            //if (defaultEdition != null)
            //{
            //    tenant.EditionId = defaultEdition.Id;
            //}
            var checkExist = await _tenantManager.FindByTenancyNameAsync(tenant.TenancyName);
            if (checkExist != null)
            {
                throw new UserFriendlyException(string.Format(L("TenancyNameIsAlreadyTaken{0}"), tenant.TenancyName));
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
                // create email setting
                //await CreateSettingEmail(tenant.Id, tenant.Name);
                // Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                adminUser.IdChiNhanhMacDinh = idChiNhanh;
                await _userManager.InitializeOptionsAsync(tenant.Id);

                if (input.IsDefaultPassword == true || string.IsNullOrEmpty(input.Password))
                {
                    CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                }
                else
                {
                    CheckErrors(await _userManager.CreateAsync(adminUser, input.Password));
                }

                await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                // Assign admin user to role!
                CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();

                // init Data
                _seedDataEntities.InnitData(tenant.Id);
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
        [NonAction]
        public Task CreateSettingEmail(int tenantId, string tenantName)
        {
            List<Setting> settings = new List<Setting>()
            {
                new Setting()
                {
                    Name = EmailSettingNames.DefaultFromDisplayName,
                    Value = tenantName,
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                },
                new Setting()
                {
                    Name = EmailSettingNames.DefaultFromAddress,
                    Value = "admin@mydomain.com",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                }
                ,new Setting()
                {
                    Name = EmailSettingNames.Smtp.UserName,
                    Value = "admin@mydomain.com",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                }
                ,
                new Setting()
                {
                    Name = EmailSettingNames.Smtp.Password,
                    Value = "",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                },
                new Setting()
                {
                    Name = EmailSettingNames.Smtp.Port,
                    Value = "587",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                },
                new Setting()
                {
                    Name = EmailSettingNames.Smtp.Host,
                    Value = "smtp.gmail.com",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                },
                new Setting()
                {
                    Name = EmailSettingNames.Smtp.Domain,
                    Value = "",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                },
                new Setting()
                {
                    Name = EmailSettingNames.Smtp.EnableSsl,
                    Value = "true",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                },
                new Setting()
                {
                    Name = EmailSettingNames.Smtp.UseDefaultCredentials,
                    Value = "false",
                    TenantId = tenantId,
                    CreationTime = DateTime.Now
                }

            };
            return _settingRepository.InsertRangeAsync(settings);
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
            tenant.EditionId = input.EditionId;
            tenant.IsTrial = input.IsTrial;
            if (tenant.IsTrial == false)
            {
                tenant.SubscriptionEndDate = input.SubscriptionEndDate;
            }
            if (!string.IsNullOrEmpty(input.ConnectionString))
            {
                tenant.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            }
            var checkExist = await Repository.FirstOrDefaultAsync(x => x.TenancyName == tenant.TenancyName && x.Id != tenant.Id);
            if (checkExist != null)
            {
                throw new UserFriendlyException(string.Format(L("TenancyNameIsAlreadyTaken{0}"), tenant.TenancyName));
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
        [AbpAuthorize(PermissionNames.Pages_Tenants_UpdateMigration)]
        public async Task UpdateMigrations()
        {
            var hostConnStr = _configuration.GetConnectionString(SPAConsts.ConnectionStringName);
            _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            var migratedDatabases = new HashSet<string>();
            var tenants = Repository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    _migrator.CreateOrMigrateForTenant(tenant);
                    migratedDatabases.Add(tenant.ConnectionString);
                }
            }
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<int> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
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

        [HttpGet]
        public async Task<PagedResultDto<TenantInfoActivityDto>> GetTenantStatusActivity(PagedTenantResultRequestDto input)
        {
            PagedResultDto<TenantInfoActivityDto> result = new PagedResultDto<TenantInfoActivityDto>();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var tenants = Repository.GetAll().OrderByDescending(x =>x.CreationTime)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword)).ToList();
            result.TotalCount = tenants.Count;
            var items = new List<TenantInfoActivityDto>();
            if(tenants!=null&& tenants.Count>0)
            {
                foreach( var tenant in tenants)
                {
                    var edition = await _editionManager.GetByIdAsync(tenant.EditionId??0);
                    using (UnitOfWorkManager.Current.SetTenantId(tenant.Id))
                    {
                        TenantInfoActivityDto rdo = new TenantInfoActivityDto();
                        rdo.Id = tenant.Id;
                        rdo.TenancyName = tenant.TenancyName;
                        rdo.Name = tenant.Name;
                        rdo.CreationTime = tenant.CreationTime;
                        rdo.SubscriptionEndDate = tenant.SubscriptionEndDate;
                        rdo.EditionName = edition != null ? edition.DisplayName : "";
                        var nhatKyThaoTac = _nhatKyThaoTacRepository.GetAll().OrderByDescending(x=>x.CreationTime).Take(1).FirstOrDefault();
                        rdo.LastActivityTime = nhatKyThaoTac != null ? nhatKyThaoTac.CreationTime : tenant.CreationTime;
                        rdo.Status = tenant.SubscriptionEndDate.HasValue==false ? "": (tenant.SubscriptionEndDate.Value < DateTime.Now?"Quá hạn":"Còn hạn");
                        items.Add(rdo);
                    }
                }
            }
            result.Items = items;
            return result;
        }

        public async Task<PagedResultDto<TenantHistoryActivityDto>> GetTenantHistoryActivity(PagedRequestDto input, int tenantId)
        {
            PagedResultDto<TenantHistoryActivityDto> result = new PagedResultDto<TenantHistoryActivityDto>();
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var checkTenant =await _tenantManager.FindByIdAsync(tenantId);
            if (checkTenant != null) {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var data = await _nhatKyThaoTacRepository.GetAllIncluding().Where(x => x.IsDeleted == false&& x.LoaiNhatKy!=LoaiThaoTacConst.Login).OrderByDescending(x => x.CreationTime).ToListAsync();
                    
                    if (!string.IsNullOrEmpty(input.Keyword))
                    {
                        data = data.Where(x => x.NoiDung.Contains(input.Keyword) || x.ChucNang.Contains(input.Keyword) || x.NoiDungChiTiet.Contains(input.Keyword)).ToList();
                    }
                    result.TotalCount = data.Count;
                    var nhatKyThaoTac = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                    result.Items = ObjectMapper.Map<List<TenantHistoryActivityDto>>(nhatKyThaoTac);
                    foreach (var item in result.Items)
                    {
                        var userId = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == item.CreatorUserId);
                        item.TenNguoiThaoTac = userId != null ? userId.FullName:"Admin";
                        if (item.IdChiNhanh.HasValue) {
                            var chinhanh = await _chiNhanhRepository.FirstOrDefaultAsync(x => x.Id == item.IdChiNhanh.Value);
                            item.ChiNhanh = chinhanh != null ? chinhanh.TenChiNhanh : "";
                        }
                        else
                        {
                            if (userId != null && userId.IdChiNhanhMacDinh.HasValue)
                            {
                                var chinhanh = await _chiNhanhRepository.FirstOrDefaultAsync(x => x.Id == userId.IdChiNhanhMacDinh.Value);
                                item.ChiNhanh = chinhanh != null ? chinhanh.TenChiNhanh : "";
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}

