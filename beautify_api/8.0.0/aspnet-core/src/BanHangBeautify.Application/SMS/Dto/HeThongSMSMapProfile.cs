using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.SMS.Dto
{
    public class HeThongSMSMapProfile : Profile
    {
        public HeThongSMSMapProfile()
        {
            CreateMap<CreateOrEditHeThongSMSDto, HeThong_SMS>().ReverseMap();
            CreateMap<HeThongSMSDto, HeThong_SMS>().ReverseMap();
            CreateMap<CreateOrEditHeThongSMSDto, HeThongSMSDto>().ReverseMap();
            CreateMap<HT_SMSBrandname, BrandnameDto>().ReverseMap();
            CreateMap<HT_SMSBrandname, PageBrandnameDto>().ReverseMap();
            CreateMap<SMS_LichSuNap_ChuyenTien, LichSuNap_ChuyenTienDto>().ReverseMap();
        }
    }
}
