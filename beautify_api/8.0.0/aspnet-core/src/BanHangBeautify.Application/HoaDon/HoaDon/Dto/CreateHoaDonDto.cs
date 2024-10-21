using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using System;
using System.Collections.Generic;

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

        public double? TongTienHangChuaChietKhau { get; set; } = 0;
        public double? PTChietKhauHang { get; set; } = 0;
        public double? TongChietKhauHangHoa { get; set; } = 0;
        public double? TongTienHang { get; set; } = 0;
        public double? PTThueHD { get; set; } = 0;
        public double? TongTienThue { get; set; } = 0;
        public double? TongTienHDSauVAT { get; set; } = 0;
        public double? PTGiamGiaHD { get; set; } = 0;
        public double? TongGiamGiaHD { get; set; } = 0;
        public double? ChiPhiTraHang { get; set; } = 0; // Áp dụng khi khách trả hàng, và khách phải trả thêm khoản phí này
        public double? TongThanhToan { get; set; } = 0;// tong tien khach can thanh toan
        public double? ChiPhiHD { get; set; } = 0;// Chi phí cửa hàng phải trả cho bên thứ 3 (VNĐ) (VD: chi phí vận chuyển)
        public string ChiPhi_GhiChu { get; set; } = string.Empty;
        public double? DiemGiaoDich { get; set; } = 0; // Số điểm khách hàng tích được khi mua hàng theo hóa đơn này
        public bool? LaHoaDonDauKy { get; set; } = false;

        public List<HoaDonChiTietDto> HoaDonChiTiet { get; set; }

    }
}
