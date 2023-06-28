using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class CreateHoaDonDto
    {
        public Guid Id { get; set; }
        public int IdLoaiChungTu { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public Guid? IdKhachHang { get; set; }
        public Guid? IdNhanVien { get; set; }
        public Guid? IdViTriPhong { get; set; }
        public Guid? IdHoaDon { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime? NgayApDung { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public string GhiChuHD { get; set; }

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
        public string ChiPhi_GhiChu { get; set; } = string.Empty;
        public float? DiemGiaoDich { get; set; } = 0; // Số điểm khách hàng tích được khi mua hàng theo hóa đơn này

        public List<HoaDonChiTietDto> HoaDonChiTiet { get; set; }
        
    }
}
