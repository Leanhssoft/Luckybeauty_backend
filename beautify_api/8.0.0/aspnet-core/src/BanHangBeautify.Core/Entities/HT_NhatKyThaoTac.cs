using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class HT_NhatKyThaoTac : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public Guid IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        [MaxLength(2000)]
        public string ChucNang { get; set; }
        public int LoaiNhatKy { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungChiTiet { get; set; }
    }
}