using Abp;
using BanHangBeautify.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Roles.Repository
{
    public interface IUserRoleRepository
    {
        Task<List<UserIdentifierDto>> GetListUser_havePermission(int tenantId, Guid idChiNhanh, string permissionsName);
    }
}
