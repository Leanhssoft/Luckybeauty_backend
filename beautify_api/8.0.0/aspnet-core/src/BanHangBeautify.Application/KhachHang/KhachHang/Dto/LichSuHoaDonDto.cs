using System;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class LichSuHoaDonDto
    {
        public string MaHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public decimal TongTienHang { get; set; }
        public decimal TongGiamGia { get; set; }

        public decimal TongPhaiTra { get; set; }

        public decimal KhachDaTra { get; set; }

        public decimal ConNo { get; set; }
        public string TrangThai { get; set; }
    }
}
