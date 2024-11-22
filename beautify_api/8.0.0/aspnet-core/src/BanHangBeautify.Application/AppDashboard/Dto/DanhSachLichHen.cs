using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;

namespace BanHangBeautify.AppDashboard.Dto
{
    public class DanhSachLichHen : CustomerBasicDto
    {
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double? GiaBan { get; set; }
        public string TenHangHoa { get; set; }
        public string TxtTrangThai { get; set; }
        public int? TrangThai { get; set; }
    }
}
