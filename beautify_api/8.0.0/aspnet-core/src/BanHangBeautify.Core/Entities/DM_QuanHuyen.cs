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
    public class DM_QuanHuyen : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(10)]
        public string MaQuanHuyen { get; set; }
        public string TenQuanHuyen { get; set; }
        public Guid? IdTinhThanh { get; set; }
        [ForeignKey("IdTinhThanh")]
        public DM_TinhThanh DM_TinhThanh { get; set; }
    }
}
