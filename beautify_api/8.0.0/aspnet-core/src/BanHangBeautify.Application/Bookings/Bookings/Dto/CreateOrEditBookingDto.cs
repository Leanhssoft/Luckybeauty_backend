using System;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class CreateOrEditBookingDto
    {
        public Guid Id { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public string StartTime { get; set; }
        public string StartHours { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid? IdNhanVien { get; set; } = null;
        public Guid IdDonViQuiDoi { get; set; }
    }
}
