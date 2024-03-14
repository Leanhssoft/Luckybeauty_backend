using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization.Sources;
using Abp.UI;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Consts;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Base;

namespace BanHangBeautify.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        INhatKyThaoTacAppService _audilogService;
        
        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager,INhatKyThaoTacAppService audilogService)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            LocalizationSourceName = SPAConsts.LocalizationSourceName;
            _audilogService = audilogService;
        }
        [NonAction]
        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();
            var checkCreate = await _roleManager.CreateAsync(role);
            CheckErrors(checkCreate);
            //CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Vai trò";
            nhatKyThaoTacDto.NoiDung = "Thêm mới vai trò: " + role.DisplayName;
            nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới vai trò: " + role.DisplayName;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
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

            return new PagedResultDto<RoleListDto>(roles.Count, ObjectMapper.Map<List<RoleListDto>>(roles));
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
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Vai trò";
            nhatKyThaoTacDto.NoiDung = "Cập nhật vai trò: " + role.DisplayName;
            nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật vai trò: " + role.DisplayName;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return MapToEntityDto(role);
        }
        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<ExecuteResultDto> CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Id.Value > 0)
            {
                return await UpdateRole(input);
            }
            else
            {
                return await CreateRole(input);
            }
        }

        protected virtual async Task<ExecuteResultDto> CreateRole(CreateOrUpdateRoleInput input)
        {
            CheckUpdatePermission();
            var role = new Role(AbpSession.TenantId, input.Name, input.DisplayName) { };
            role.SetNormalizedName();
            var checkError = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.DisplayName);
            if (checkError.Status=="error")
            {
                return checkError;
            }
            else
            {
                CheckErrors(await _roleManager.CreateAsync(role));
                await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
                await UpdateGrantedPermissionsAsync(role, input.GrantedPermissions);
                checkError.Message = "Thêm mới thành công!";
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
                nhatKyThaoTacDto.ChucNang = "Vai trò";
                nhatKyThaoTacDto.NoiDung = "Thêm mới vai trò: " + role.DisplayName;
                nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới vai trò: " + role.DisplayName;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return checkError;
            }
        }
        public virtual async Task<ExecuteResultDto> CheckDuplicateRoleNameAsync(
            int? expectedRoleId,
            string name,
            string displayName)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role != null && role.Id != expectedRoleId)
            {
                return new ExecuteResultDto()
                {
                    Status = "error",
                    Message = string.Format(L("RoleNameIsAlreadyTaken{0}"), name),
                    Detail = displayName
                };
            }

            role = await Repository.FirstOrDefaultAsync(x => x.DisplayName == displayName);
            if (role != null && role.Id != expectedRoleId)
            {
               return new ExecuteResultDto() { 
               Status="error",
               Message=string.Format(L("RoleDisplayNameIsAlreadyTaken{0}"),displayName),
               Detail = displayName
               };
            }

            return new ExecuteResultDto()
            {
                Status = "success"
            };
        }
        protected virtual async Task<ExecuteResultDto> UpdateRole(CreateOrUpdateRoleInput input)
        {
            CheckUpdatePermission();
            var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
            role.Name = input.Name;
            role.DisplayName = input.DisplayName;
            role.Description = input.Description;
            var checkError = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.DisplayName);
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissions);
            if (checkError.Status == "error")
            {
                return checkError;
            }
            else
            {
                CheckErrors(await _roleManager.UpdateAsync(role));
                await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
                await UpdateGrantedPermissionsAsync(role, input.GrantedPermissions);
                checkError.Message = "Cập nhật thành công!";
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
                nhatKyThaoTacDto.ChucNang = "Vai trò";
                nhatKyThaoTacDto.NoiDung = "Cập nhật vai trò: " + role.DisplayName;
                nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật vai trò: " + role.DisplayName;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return checkError;
            }
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
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
            nhatKyThaoTacDto.ChucNang = "Vai trò";
            nhatKyThaoTacDto.NoiDung = "Xóa vai trò: " + role.DisplayName;
            nhatKyThaoTacDto.NoiDungChiTiet = "Xóa vai trò: " + role.DisplayName;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
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
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Vai trò";
                nhatKyThaoTacDto.NoiDung = "Xóa vai trò: " + role.DisplayName;
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa vai trò: " + role.DisplayName;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                result = true;
            }
            catch (System.Exception)
            {
                result = false;
            }
            return result;

        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<int> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            var checkExists = await Repository.GetAll().Where(x=>ids.Contains(x.Id)).ToListAsync();
            if (checkExists != null && checkExists.Count > 0)
            {
                Repository.RemoveRange(checkExists);
                result.Status = "success";
                result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Vai trò";
                nhatKyThaoTacDto.NoiDung = "Xóa các vai trò: " + string.Format(", "+ checkExists.Select(x=>x.DisplayName).ToList());
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa các vai trò: " + string.Format(", " + checkExists.Select(x => x.DisplayName).ToList());
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
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
                Description = role.Description,
                DisplayName = role.DisplayName,
                Name = role.Name,
                Id = role.Id,
                GrantedPermissions = grantedPermissions.Select(p => p.Name).ToList()
            };
        }
    }
}

