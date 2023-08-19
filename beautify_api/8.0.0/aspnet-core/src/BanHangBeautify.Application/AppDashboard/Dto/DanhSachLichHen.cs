using System;

namespace BanHangBeautify.AppDashboard.Dto
{
    public class DanhSachLichHen
    {
        public string Avatar { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TongTien { get; set; }
        public string DichVu { get; set; }
        public string TrangThai { get; set; }
    }
}
