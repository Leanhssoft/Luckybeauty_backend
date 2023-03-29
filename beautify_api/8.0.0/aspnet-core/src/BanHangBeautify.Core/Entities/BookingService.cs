using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class BookingService : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdDonViQuiDoi { get; set; }
        [ForeignKey("IdDonViQuiDoi")]
        public DM_DonViQuiDoi DM_DonViQuiDoi { get; set; }
        public Guid IdBooking { get; set; }
        [ForeignKey("IdBooking")]
        public Booking Booking { get; set; }
    }
}
