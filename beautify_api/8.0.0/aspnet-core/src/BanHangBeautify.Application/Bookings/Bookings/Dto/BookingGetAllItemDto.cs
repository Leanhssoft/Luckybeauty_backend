using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class BookingGetAllItemDto
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Customer { get; set; }
        public Guid SourceId { get; set; }
        public string Employee { get; set; }
        public string Services { get; set; }
        public string Color { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime BookingDate { get; set; }
    }
    public class BookingDetailDto
    {
        public Guid IdBooking { get; set; }
        public Guid? IdKhachHang { get; set; }
        public int TrangThai { get; set; }// trangthai book
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? BookingDate { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public double? GiaBan { get; set; }
        public string TxtTrangThaiBook { get; set; }
    }

    public class BookingDetailOfCustometDto
    {
        public Guid IdBooking { get; set; }
        public Guid? IdKhachHang { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public int TrangThai { get; set; }
        public string TxtTrangThaiBook { get; set; }
        public List<BookingDetailDto> Details { get; set; }
    }
}
