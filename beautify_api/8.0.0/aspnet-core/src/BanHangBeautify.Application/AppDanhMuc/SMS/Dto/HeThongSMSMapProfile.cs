using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.SMS.Dto
{
    public class HeThongSMSMapProfile : Profile
    {
        public HeThongSMSMapProfile()
        {
            CreateMap<CreateOrEditHeThongSMSDto, HeThong_SMS>().ReverseMap();
            CreateMap<HeThongSMSDto, HeThong_SMS>().ReverseMap();
            CreateMap<CreateOrEditHeThongSMSDto, HeThongSMSDto>().ReverseMap();
        }
    }
}
