using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class BH_HoaDon_Anh : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required]
        public int TenantId { get; set; } = 1;
        public Guid IdHoaDon { get; set; }
        [ForeignKey("IdHoaDon")]
        public BH_HoaDon BH_HoaDon { get; set; }
        [MaxLength(4000)]
        public string URLAnh { get; set; }
    }
}
