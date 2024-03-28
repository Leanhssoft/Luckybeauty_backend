using Abp.Domain.Entities.Auditing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class Zalo_ButtonDetail : FullAuditedEntity<Guid>
    {
        public Guid IdElement { get; set; }
        [ForeignKey("IdElement")]
        public Zalo_Element Zalo_Element { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Payload { get; set; }
        public string ImageIcon { get; set; }
        public byte? ThuTuSapXep { get; set; }
    }
}
