using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_Phong : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required] public int TenantId { get; set; } = 1;
        [MaxLength(50)]
        public string MaPhong { get; set; }
        [Required, MaxLength(256)]
        public string TenPhong { get; set; }
        public Guid? IdViTri { get; set; }
        [ForeignKey("IdViTri")]
        public DM_ViTriPhong DM_ViTriPhong { get; set; }
    }
}
