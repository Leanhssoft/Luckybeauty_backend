using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.Checkin.Dto
{
    public class KHCheckInMapProfile : Profile
    {
        public KHCheckInMapProfile()
        {
            CreateMap<KH_CheckIn, KHCheckInDto>().ReverseMap();
            CreateMap<KH_CheckIn, PageKhachHangCheckingDto>().ReverseMap();
            CreateMap<Booking_CheckIn_HoaDon, CheckInHoaDonDto>().ReverseMap();
        }
    }
}
