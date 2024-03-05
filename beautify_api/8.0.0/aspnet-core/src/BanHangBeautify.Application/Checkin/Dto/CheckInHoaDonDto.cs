using System;

namespace BanHangBeautify.Checkin.Dto
{
    public class CheckInHoaDonDto
    {
        public Guid IdCheckIn { get; set; }
        public Guid? IdHoaDon { get; set; }
        public Guid? IdBooking { get; set; }
        public int? TrangThai { get; set; } = 1;
    }
}
