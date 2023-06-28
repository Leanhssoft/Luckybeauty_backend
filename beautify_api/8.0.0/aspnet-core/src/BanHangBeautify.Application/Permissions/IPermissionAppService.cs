using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BanHangBeautify.Permissions.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        Task<GetPermissionDto> GetAllPermissionByRole(long UserId);
        ListResultDto<PermissionTreeDto> GetAllPermissions();
    }
}
