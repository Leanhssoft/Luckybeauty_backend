using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [NonAction]
        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        public async Task<PagedResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new PagedResultDto<RoleListDto>(roles.Count,ObjectMapper.Map<List<RoleListDto>>(roles));
        }
        [NonAction]
        public override async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }
        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create,PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<RoleDto> CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Id.Value>0)
            {
                return await UpdateRole(input);
            }
            else
            {
                return await CreateRole(input);
            }
        }
        
        protected virtual async Task<RoleDto> CreateRole(CreateOrUpdateRoleInput input)
        {
            CheckUpdatePermission();
            var role = new Role(AbpSession.TenantId,input.Name, input.DisplayName) { };
            role.SetNormalizedName();
            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissions);
            return MapToEntityDto(role);
        }
        protected virtual async Task<RoleDto> UpdateRole(CreateOrUpdateRoleInput input)
        {
            CheckUpdatePermission();
            var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
            role.DisplayName = input.DisplayName;
            role.Description = input.Description;
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissions);
            return MapToEntityDto(role);
        }
        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => grantedPermissionNames.Contains(p.Name))
                .ToList();
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
        [NonAction]
        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Delete)]
        public async Task<bool> DeleteRole(EntityDto<int> input)
        {
            bool result = false;
            try
            {
                var role = await _roleManager.FindByIdAsync(input.Id.ToString());
                var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);
                foreach (var user in users)
                {
                    CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
                }
                role.IsDeleted = true;
                role.DeletionTime = DateTime.Now;
                role.DeleterUserId = AbpSession.UserId;
                await _roleManager.UpdateAsync(role);
                result= true;
            }
            catch (System.Exception)
            {
                result = false;
            }
            return result;
           
        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            return Repository.GetAllIncluding(x => x.Permissions)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword));
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<CreateOrUpdateRoleInput> GetRoleForEdit(EntityDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
            return new CreateOrUpdateRoleInput
            {
                Description= role.Description,
                DisplayName = role.DisplayName,
                Name = role.Name,
                Id= role.Id,
                GrantedPermissions = grantedPermissions.Select(p => p.Name).ToList()
            };
        }
    }
}

