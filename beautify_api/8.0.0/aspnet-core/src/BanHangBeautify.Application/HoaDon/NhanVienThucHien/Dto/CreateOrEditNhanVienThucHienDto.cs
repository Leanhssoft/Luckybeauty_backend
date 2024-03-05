using System;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Dto
{
    public class CreateOrEditNhanVienThucHienDto
    {
        public Guid Id { set; get; }
        public Guid IdNhanVien { set; get; }
        public Guid? IdHoaDon { set; get; }
        public Guid? IdHoaDonChiTiet { set; get; }
        public Guid? IdQuyHoaDon { set; get; }
        public double? PTChietKhau { set; get; }
        public double? TienChietKhau { set; get; }
        public bool? ChiaDeuChietKhau { set; get; } = false;
        public float? HeSo { set; get; }
        public bool? TinhHoaHongTruocCK { set; get; } = false;
        public byte? LoaiChietKhau { set; get; }
        public string TenNhanVien { get; set; }
    }
}
