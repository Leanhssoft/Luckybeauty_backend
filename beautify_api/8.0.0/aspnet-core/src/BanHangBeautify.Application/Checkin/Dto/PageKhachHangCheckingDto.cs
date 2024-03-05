using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;

namespace BanHangBeautify.Checkin.Dto
{
    public class PageKhachHangCheckingDto : KhachHangView
    {
        public Guid? IdCheckIn { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid? IdBooking { get; set; }
        public string DateCheckIn { get; set; }
        public string TimeCheckIn { get; set; }
        public string TxtTrangThaiCheckIn { get; set; }
    }
}
