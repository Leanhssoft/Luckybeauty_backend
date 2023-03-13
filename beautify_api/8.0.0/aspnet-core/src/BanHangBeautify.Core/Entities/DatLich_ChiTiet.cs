using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.Entities;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Data.Entities
{
    public class DatLich_ChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public Guid IdDatLich { set; get; }
        [ForeignKey("IdDatLich")]
        public DatLich DatLich { get; set; }
        public Guid IdHangHoa { get; set; }
        [ForeignKey("IdHangHoa")]
        public DM_HangHoa DM_HangHoa { get; set; }
       
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public int TrangThai { get; set; }
        public bool IsDelete { get; set; }
        public int TenantId { get; set; }
    }
}
