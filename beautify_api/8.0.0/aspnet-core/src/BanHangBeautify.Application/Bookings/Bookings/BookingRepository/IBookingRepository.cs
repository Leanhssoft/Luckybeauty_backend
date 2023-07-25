using BanHangBeautify.Bookings.Bookings.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.BookingRepository
{
    public interface IBookingRepository
    {
        Task<List<BookingGetAllItemDto>> GetAllBooking(PagedBookingResultRequestDto input, int tenantId, DateTime timeFrom, DateTime timeTo);
        Task<List<BookingDetailDto>> GetKhachHang_Booking(BookingRequestDto input);
        Task<List<BookingDetailDto>> GetInforBooking_byID(Guid idBooking);
    }
}
