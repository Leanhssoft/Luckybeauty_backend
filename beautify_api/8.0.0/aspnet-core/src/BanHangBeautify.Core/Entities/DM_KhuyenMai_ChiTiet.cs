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
        [ForeignKey("IdKhuyenMai")]
        public DM_KhuyenMai DM_KhuyenMai { get; set; }

        public byte? STT { get; set; } = 1;
        public float? TongTienHang { get; set; } = 0;
        public float? GiamGia { get; set; } = 0;
        public bool? GiamGiaTheoPhanTram { get; set; } = true;
        public Guid? IdNhomHangMua { get; set; }
        [ForeignKey("IdNhomHangMua")]
        public DM_NhomHangHoa DM_NhomHangHoaMua { get; set; }
        public Guid? IdDonViQuiDoiMua { get; set; }
        [ForeignKey("IdDonViQuiDoiMua")]
        public DM_DonViQuiDoi DM_DonViQuiDoiMua { get; set; }
        public Guid? IdNhomHangTang { get; set; }
        [ForeignKey("IdNhomHangTang")]
        public DM_NhomHangHoa DM_NhomHangHoaTang { get; set; }
        public Guid? IdDonViQuiDoiTang { get; set; }
        [ForeignKey("IdDonViQuiDoiTang")]
        public DM_DonViQuiDoi DM_DonViQuiDoiTang { get; set; }
        public float? SoLuongMua { get; set; } = 0;
        public float? SoLuongTang { get; set; } = 0;
        public float? GiaKhuyenMai { get; set; } = 0;
    }
}
