using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class BookingMapProfile : Profile
    {
        public BookingMapProfile()
        {
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, UpdateBookingDto>().ReverseMap();
            CreateMap<Booking, CancelBookingDto>().ReverseMap();
        }
    }
}
