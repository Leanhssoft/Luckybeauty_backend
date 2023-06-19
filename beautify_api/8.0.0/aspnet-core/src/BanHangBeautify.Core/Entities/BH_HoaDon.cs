using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using BanHangBeautify.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class BH_HoaDon : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required]
        public int TenantId { get; set; } = 1;
        public int IdLoaiChungTu { get; set; }
        [ForeignKey("IdLoaiChungTu")]
        public DM_LoaiChungTu DM_LoaiChungTu { get; set; }
        [Required, MaxLength(256)]
        public string MaHoaDon { get; set; } = string.Empty;
        [Required]
        public DateTime NgayLapHoaDon { get; set; } = DateTime.Now;
        public DateTime? NgayApDung { get; set; } = DateTime.Now;
        public DateTime? NgayHetHan { get; set; }
        public Guid? IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public Guid? IdKhachHang { get; set; }
        [ForeignKey("IdKhachHang")]
        public DM_KhachHang DM_KhachHang { get; set; }
        public Guid? IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
        public Guid? IdPhong { get; set; }
        [ForeignKey("IdPhong")]
        public DM_Phong DM_Phong { get; set; }
        public Guid? IdHoaDon { get; set; }
        [ForeignKey("IdHoaDon")]
        public BH_HoaDon BH_HoaDonParent { get; set; }
        public float? TongTienHangChuaChietKhau { get; set; } = 0;
        public float? PTChietKhauHang { get; set; } = 0;
        public float? TongChietKhauHangHoa { get; set; } = 0;
        public float? TongTienHang { get; set; } = 0;
        public float? PTThueHD { get; set; } = 0;
        public float? TongTienThue { get; set; } = 0;
        public float? TongTienHDSauVAT { get; set; } = 0;
        public float? PTGiamGiaHD { get; set; } = 0;
        public float? TongGiamGiaHD { get; set; } = 0;
        public float? ChiPhiTraHang { get; set; } = 0; // Áp dụng khi khách trả hàng, và khách phải trả thêm khoản phí này
        public float? TongThanhToan { get; set; } = 0;// tong tien khach can thanh toan
        public float? ChiPhiHD { get; set; } = 0;// Chi phí cửa hàng phải trả cho bên thứ 3 (VNĐ) (VD: chi phí vận chuyển)
        [MaxLength(4000)]
        public string ChiPhi_GhiChu { get; set; } = string.Empty;
        public float? DiemGiaoDich { get; set; } = 0; // Số điểm khách hàng tích được khi mua hàng theo hóa đơn này
        [MaxLength(4000)]
        public string GhiChuHD { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 3;// 0.Xóa, 1.Tạm lưu, 2.Đang xử lý, 3.Hoàn thành
        //public virtual ICollection<BH_HoaDon_ChiTiet> BH_HoaDon_ChiTiet { get; set; }
    }
}
