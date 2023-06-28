using AutoMapper;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;

namespace BanHangBeautify.DataExporting.Checkin.Dto
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
