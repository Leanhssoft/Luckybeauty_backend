using BanHangBeautify.Permissions.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Permissions
{
    public interface IPermissionAppService
    {
        Task<GetPermissionDto> GetAllPermissionByRole(long UserId);
    }
}
