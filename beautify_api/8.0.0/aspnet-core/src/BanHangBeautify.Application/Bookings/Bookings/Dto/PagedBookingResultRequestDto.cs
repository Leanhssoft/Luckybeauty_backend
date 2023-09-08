using System;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class PagedBookingResultRequestDto
    {
        public Guid IdChiNhanh { get; set; }
        public DateTime DateSelected { get; set; }
        public string TypeView { get; set; }
        public Guid? IdNhanVien { get; set; }
        public Guid? IdDichVu { get; set; }
    }
    public class BookingRequestDto : ParamSearch
    {
        public int? TrangThaiBook { get; set; } = 3; // 3.all, 1.chua xacnhan, 2.da xacnhan, 0.xoa
    }
}
