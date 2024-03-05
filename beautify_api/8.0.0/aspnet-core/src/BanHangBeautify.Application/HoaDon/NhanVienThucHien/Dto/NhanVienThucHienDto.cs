using System;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Dto
{
    public class NhanVienThucHienDto
    {
        public Guid? IdNhanVien { set; get; }
        public Guid? IdHoaDon { set; get; }
        public Guid? IdChiTietHoaDon { set; get; }
        public Guid? IdQuyHoaDon { set; get; }
        public double? PTChietKhau { set; get; }
        public double? TienChietKhau { set; get; }
        public bool? ChiaDeuChietKhau { set; get; } = false;
        public double? HeSo { set; get; } = 1;
        public bool? TinhHoaHongTruocCK { set; get; } = false;
        public byte? LoaiChietKhau { set; get; } = 1;
    }
}
