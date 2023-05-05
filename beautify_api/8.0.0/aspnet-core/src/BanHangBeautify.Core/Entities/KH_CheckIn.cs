using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class KH_CheckIn : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; } = 1;
        public Guid IdChiNhanh { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid? IdBooking { get; set; }
        public DateTime DateCheckIn { get; set; }
        public int TrangThai { get; set; } = 1; // 0. Check in nhưng đợi lâu quá --> Cancel, 1. check in & làm dịch vụ (mặc định)
    }
}
