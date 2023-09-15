using Abp.Application.Editions;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Roles.Dto
{
    public class CreateOrUpdateUserRoleDto: UserRole
    {
        //public int? TenantId { get; set; }
        //public long UserId { get; set; }
        //public int RoleId { get; set; }
        public Guid? IdChiNhanh { get; set; }
    }
}
