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
    public class DM_ViTriPhong:FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required] public int TenantId { get; set; } = 1;
        [MaxLength(50)]
        public string MaViTriPhong { get; set; }
        [Required, MaxLength(256)]
        public string TenViTriPhong { get; set; }
    }
}
