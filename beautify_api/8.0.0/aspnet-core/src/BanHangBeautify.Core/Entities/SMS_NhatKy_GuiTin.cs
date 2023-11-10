using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class SMS_NhatKy_GuiTin : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdHeThongSMS { get; set; }
        [ForeignKey("IdHeThongSMS")]
        public HeThong_SMS HeThong_SMS { get; set; }
        public Guid? IdHoaDon { get; set; }
        [ForeignKey("IdHoaDon")]
        public BH_HoaDon BH_HoaDon { get; set; }
        public Guid? IdBooking { get; set; }
        [ForeignKey("IdBooking")]
        public Booking Booking { get; set; }
        public DateTime? ThoiGianTu { get; set; }
        public DateTime? ThoiGianDen { get; set; }
    }
}
