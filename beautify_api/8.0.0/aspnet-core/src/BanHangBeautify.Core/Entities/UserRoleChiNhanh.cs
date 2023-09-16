using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    [Table("AbpUserRoles")]
    public class UserRoleChiNhanh:UserRole
    {
        public Guid? IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }

        public UserRoleChiNhanh(int? tenantId, long userId, int roleId, Guid? idChiNhanh= null) : base(tenantId, userId, roleId)
        {
            this.IdChiNhanh = idChiNhanh;
        }
    }
}
