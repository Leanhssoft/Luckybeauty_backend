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
    public class DanhGia :FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdDatLich { get; set; }
        [ForeignKey("IdDatLich")]
        public DatLich DatLich { get; set; }
        public float SoSaoDanhGia { get; set; }
        public string GhiChu { get; set; }
    }
}
