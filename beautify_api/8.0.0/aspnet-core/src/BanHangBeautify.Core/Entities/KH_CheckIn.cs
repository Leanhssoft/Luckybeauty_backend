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
        public Guid? IdChiNhanh { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid? IdBooking { get; set; }
        public DateTime DateTimeCheckIn { get; set; }// ngay check in yyyy-mm-dd hh:mm:ss
        public DateOnly DateCheckIn { get { return DateOnly.FromDateTime(DateTimeCheckIn); } }// ngay check in yyyy-mm-dd
        public TimeOnly TimeCheckIn { get { return TimeOnly.FromDateTime(DateTimeCheckIn); } } // thoigian checkin 1205.22:47:09.5550000
        [MaxLength(4000)]
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1; // 0. Check in nhưng đợi lâu quá --> Cancel, 1. check in & làm dịch vụ (mặc định)
    }
}
