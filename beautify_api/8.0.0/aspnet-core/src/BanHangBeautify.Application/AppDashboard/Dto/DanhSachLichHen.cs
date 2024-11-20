using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;

namespace BanHangBeautify.AppDashboard.Dto
{
    public class DanhSachLichHen : CustomerBasicDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double? GiaBan { get; set; }
        public string TenHangHoa { get; set; }
        public string TrangThai { get; set; }
    }
}
