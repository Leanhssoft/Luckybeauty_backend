using System;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class KhachHangDetailDto
    {
        public Guid Id { get; set; }
        public Guid? IdNhomKhach { get; set; }
        public Guid? IdLoaiKhach { get; set; }
        public Guid? IdNguonKhach { get; set; }
        public string Avatar { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public float? TongTichDiem { get; set; }
        public string MaSoThue { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string TenLoaiKhachHang { get; set; }
        public string TenNhomKhach { get; set; }
        public string TenNguonKhach { get; set; }
    }
    public class CustomerDetail_FullInfor : KhachHangDetailDto
    {
        public int? SoLanBooking { get; set; } = 0;
        public int? SoLanCheckIn { get; set; } = 0;
        public double? TongChiTieu { get; set; } = 0;
        public double? ConNo { get; set; } = 0;
        public DateTime? cuocHenGanNhat { get; set; }
    }
}
