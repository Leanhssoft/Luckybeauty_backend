using System;

namespace BanHangBeautify.Bookings.BookingColor.Dto
{
    public class CreateOrEditBookingColor
    {
        public Guid Id { get; set; }
        public int TrangThai { get; set; }
        public string MaMau { get; set; }
    }
}
