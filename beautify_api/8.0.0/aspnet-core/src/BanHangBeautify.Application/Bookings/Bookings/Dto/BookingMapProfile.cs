using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class BookingMapProfile: Profile
    {
        public BookingMapProfile()
        {
            CreateMap<Booking,BookingDto>().ReverseMap();
            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, UpdateBookingDto>().ReverseMap();
            CreateMap<Booking, CancelBookingDto>().ReverseMap();
        }
    }
}
