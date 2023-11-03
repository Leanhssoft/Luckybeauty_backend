using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;

namespace BanHangBeautify.SMS.Dto
{
    public class PageKhachHangSMSDto : CustomerBasicDto
    {
        public DateTime? NgaySinh { set; get; } = null;
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { set; get; } = null;
        public string TenHangHoa { get; set; }
        public DateTime? BookingDate { set; get; } = null;
        public string ThoiGianHen { set; get; } // từ HH:mm - HH:mm
        public string STrangThaiGuiTinNhan { get; set; }
    }
}
