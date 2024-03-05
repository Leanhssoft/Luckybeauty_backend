using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class Booking_CheckIn_HoaDon : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdCheckIn { get; set; }
        [ForeignKey("IdCheckIn")]
        public KH_CheckIn KH_CheckIn { get; set; }
        public Guid? IdBooking { get; set; }
        [ForeignKey("IdBooking")]
        public Booking Booking { get; set; }
        public Guid? IdHoaDon { get; set; }
        [ForeignKey("IdHoaDon")]
        public BH_HoaDon BH_HoaDon { get; set; }
        public int TrangThai { get; set; } = 1;// thêm để sau này có khi dùng đến
    }
}
