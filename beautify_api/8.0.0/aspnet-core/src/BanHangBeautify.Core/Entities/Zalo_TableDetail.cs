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
    public class Zalo_TableDetail : FullAuditedEntity<Guid>
    {
        public Guid IdElement { get; set; }
        [ForeignKey("IdElement")]
        public Zalo_Element Zalo_Element { get; set; }
        [MaxLength(35)]
        public string Key { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
        public byte? ThuTuSapXep { get; set; }
    }
}
