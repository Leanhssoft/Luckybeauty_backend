using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DanhGia : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdBooking { get; set; }
        [ForeignKey("IdBooking")]
        public Booking Booking { get; set; }
        public float SoSaoDanhGia { get; set; }
        public string GhiChu { get; set; }
    }
}
