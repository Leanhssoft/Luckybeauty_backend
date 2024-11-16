using Abp.Application.Services.Dto;
using BanHangBeautify.Bookings.Bookings.Dto;
using BanHangBeautify.Users.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.BookingRepository
{
    public interface IBookingRepository
    {
        Task<PagedResultDto<BookingInfoDto>> GetAllBooking(PagedBookingResultRequestDto input);
        Task<PagedResultDto<BookingDetailDto>> GetKhachHang_Booking(BookingRequestDto input);
        Task<List<BookingDetailDto>> GetInforBooking_byID(List<Guid> arrIdBooking);
        Task<BookingInfoDto> GetBookingInfo(Guid id, int tenantId);
        Task<string> FnGetBookingCode(Guid? idChiNhanh, int idLoaiChungTu);
    }
}
