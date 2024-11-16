using System;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class PagedBookingResultRequestDto : ParamSearch
    {
        public Guid? IdNhanVien { get; set; }
        public Guid? IdDichVu { get; set; }
    }
    public class BookingRequestDto : ParamSearch
    {
        public int? TrangThaiBook { get; set; } = 3; // 3.all, 1.chua xacnhan, 2.da xacnhan, 0.xoa
    }
}
