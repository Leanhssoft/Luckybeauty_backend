using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
