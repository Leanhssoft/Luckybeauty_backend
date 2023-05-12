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
    public class DM_KhuyenMai_ChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdKhuyenMai { get; set; }
        [ForeignKey(nameof(IdKhuyenMai))]
        public DM_KhuyenMai DM_KhuyenMai { get; set; }

        public byte STT { get; set; }
        public decimal TongTienHang { get; set; }
        public decimal GiamGia { get; set; }
        public bool GiamGiaTheoPhanTram { get; set; }
        public Guid? IdNhomHangMua { get; set; }
        public Guid? IdDonViQuiDoiMua { get; set; }
        public Guid? IdNhomHangTang { get; set; }
        public Guid? IdDonViQuiDoiTang { get; set; }
        public decimal SoLuongMua { get; set; }
        public decimal SoLuongTang { get; set; }
        public decimal GiaKhuyenMai { get; set; }
    }
}
