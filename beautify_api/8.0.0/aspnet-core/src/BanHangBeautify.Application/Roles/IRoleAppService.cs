﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BanHangBeautify.Roles.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Roles
{
    public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>
    {
        Task<ListResultDto<PermissionDto>> GetAllPermissions();

        Task<CreateOrUpdateRoleInput> GetRoleForEdit(EntityDto input);

        Task<PagedResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);
    }
}
