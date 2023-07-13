using BanHangBeautify.Bookings.Bookings.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings
{
    public interface IBookingAppService
    {
        public Task<List<BookingGetAllItemDto>> GetAll(PagedBookingResultRequestDto input);
    }
}
