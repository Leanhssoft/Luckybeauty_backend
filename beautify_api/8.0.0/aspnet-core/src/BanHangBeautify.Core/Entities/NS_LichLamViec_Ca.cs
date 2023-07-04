using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class NS_LichLamViec_Ca:FullAuditedEntity<Guid>,IMustHaveTenant
    {
        public int TenantId { set; get; }
        public Guid IdCaLamViec{set;get;}
        [ForeignKey("IdCaLamViec")]
        public NS_CaLamViec NS_CaLamViec { get; set; }
        [ForeignKey("IdLichLamViec")]
        public Guid IdLichLamViec { set; get; }
        public NS_LichLamViec NS_LichLamViec { get; set; }

        public DateTime NgayLamViec { get; set; }
    }
}
