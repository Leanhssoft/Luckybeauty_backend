using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class Zalo_KhachHangThanhVien: FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string ZOAUserId { get; set; }
        [MaxLength(100)]
        public string DisplayName { get; set; }// tên hiển thị trên tk zalo
        [MaxLength(50)]
        public string UserIdByApp { get; set; }
        public bool? UserIsFollower { get; set; }// khách hàng quan tâm/chưa quan tâm OA
        public string Avatar { get; set; }
    }
}
