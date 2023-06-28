using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.BookingColor.Dto
{
    public class BookingColorMapProfile:Profile
    {
        public BookingColorMapProfile()
        {
            CreateMap<BookingColorDto,Booking_Color>().ReverseMap(); 
            CreateMap<CreateOrEditBookingColor,Booking_Color>().ReverseMap();
            CreateMap<BookingColorDto,CreateOrEditBookingColor>().ReverseMap();
        }
    }
}
