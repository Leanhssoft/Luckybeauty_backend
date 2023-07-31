using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.Bookings.BookingColor.Dto
{
    public class BookingColorMapProfile : Profile
    {
        public BookingColorMapProfile()
        {
            CreateMap<BookingColorDto, Booking_Color>().ReverseMap();
            CreateMap<CreateOrEditBookingColor, Booking_Color>().ReverseMap();
            CreateMap<BookingColorDto, CreateOrEditBookingColor>().ReverseMap();
        }
    }
}
