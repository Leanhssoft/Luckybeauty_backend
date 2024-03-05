using System;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class BookingDto
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string NoiDung { get; set; }
        public string Color { get; set; }
        public Guid Id { get; set; }
    }
}
