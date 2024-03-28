using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class Zalo_Template : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; } = 1;
        public byte IdLoaiTin { get; set; }
        [MaxLength(50)]
        public string TemplateType { get; set; }
        [MaxLength(10)]
        public string Language { get; set; }
        public bool? IsDefault { get; set; }
    }
}
