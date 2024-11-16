using Abp.Application.Services.Dto;
using BanHangBeautify.Bookings.Bookings.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings
{
    public interface IBookingAppService
    {
        public Task<PagedResultDto<BookingInfoDto>> GetAll(PagedBookingResultRequestDto input);
    }
}
