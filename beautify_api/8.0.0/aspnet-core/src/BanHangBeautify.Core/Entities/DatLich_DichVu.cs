using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.Data.Entities;
using System.Reflection.PortableExecutable;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DatLich_DichVu : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public Guid IdHangHoa { get; set; }
        public DM_HangHoa DM_HangHoa { get; set; }
        public Guid IdDatLich { get; set; }
        [ForeignKey("IdDatLichChiTiet")]
        public DatLich_ChiTiet DatLich_ChiTiet { get; set; }
    }
}
