using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DatLich : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdKhachHang { set; get; }
        [ForeignKey("IdKhachHang")]
        public DM_KhachHang DM_KhachHang { get; set; }

        public DateTime NgayDatLich { get; set; }
        public int TrangThai { get; set; }
        public decimal TongTien { get; set; }
        public decimal GiamGia { set; get; }
    }
}
