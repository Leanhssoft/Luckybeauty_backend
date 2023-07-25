using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_ViTriPhong : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required] public int TenantId { get; set; } = 1;
        [MaxLength(50)]
        public string MaViTriPhong { get; set; }
        [Required, MaxLength(256)]
        public string TenViTriPhong { get; set; }
    }
}
