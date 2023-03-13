using BanHangBeautify.Permissions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Permissions
{
    public interface IPermissionAppService
    {
        Task<GetPermissionDto> GetAllPermissionByRole(long UserId);
    }
}
