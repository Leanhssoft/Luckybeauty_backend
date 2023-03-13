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
    public class NS_CaLamViec : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { set; get; }
        [MaxLength(50)]
        public string MaCa { set; get; }
        [MaxLength(256)]
        public string TenCa { set; get; }
        public DateTime GioVao { set; get; }
        public DateTime GioRa { set; get; }
        public float TongGioCong { set; get; }
        public int TrangThai { set; get; }
    }
}
