using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class Zalo_Element : FullAuditedEntity<Guid>
    {
        public Guid IdTemplate { get; set; }
        [ForeignKey("IdTemplate")]
        public Zalo_Template Zalo_Template { get; set; }
        [MaxLength(50)]
        public string ElementType { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public bool? IsImage { get; set; }
        public string Content { get; set; }
    }
}
