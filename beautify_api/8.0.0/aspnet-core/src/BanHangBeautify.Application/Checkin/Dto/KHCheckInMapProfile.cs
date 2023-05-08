using AutoMapper;
using BanHangBeautify.CheckIn.Dto;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;

namespace BanHangBeautify.Checkin.Dto
{
    public class KHCheckInMapProfile : Profile
    {
        public KHCheckInMapProfile()
        {
            CreateMap<KH_CheckIn, KHCheckInDto>().ReverseMap();
            CreateMap<KH_CheckIn, PageKhachHangCheckingDto>().ReverseMap();
        }
    }
}
