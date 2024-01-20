using System;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class LichSuHoaDonDto
    {
        public string MaHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public double? TongTienHang { get; set; }
        public double? TongGiamGia { get; set; }
        public double? TongPhaiTra { get; set; }
        public double? KhachDaTra { get; set; }
        public double? ConNo { get; set; }
        public string TxtTrangThai { get; set; }
        public string TrangThai { get; set; }
    }
}
