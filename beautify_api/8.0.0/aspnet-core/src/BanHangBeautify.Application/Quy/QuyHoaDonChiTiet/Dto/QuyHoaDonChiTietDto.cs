using System;

namespace BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto
{
    public class QuyHoaDonChiTietDto
    {
        public Guid Id { set; get; }
        public Guid IdQuyHoaDon { set; get; }
        public Guid? IdHoaDonLienQuan { set; get; }
        public Guid? IdKhachHang { set; get; }
        public Guid? IdNhanVien { set; get; }
        public Guid? IdTaiKhoanNganHang { set; get; }
        public Guid? IdKhoanThuChi { set; get; }
        public double? LaPTChiPhiNganHang { get; set; } = 0;
        public double? ChiPhiNganHang { get; set; } = 0;
        public double? ThuPhiTienGui { get; set; } = 0;
        public double? DiemThanhToan { get; set; } = 0;
        public byte HinhThucThanhToan { get; set; } = 1;
        public double? TienThu { get; set; } = 0;
    }
}
