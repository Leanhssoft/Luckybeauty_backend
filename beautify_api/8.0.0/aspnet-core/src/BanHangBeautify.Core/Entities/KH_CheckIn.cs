using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class KH_CheckIn : FullAuditedEntity<Guid>,IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdChiNhanh { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid IdBooking { get; set; }
        public DateTime DateCheckIn { get; set; }
        public decimal WaitMinutes { get; set; }
        public int TrangThai { get; set; }
    }
}
