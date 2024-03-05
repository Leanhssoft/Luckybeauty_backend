using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.Phong.Dto
{
    public class PhongMapProfile : Profile
    {
        public PhongMapProfile()
        {
            CreateMap<PhongDto, DM_Phong>().ReverseMap();
            CreateMap<CreateOrEditPhongDto, DM_Phong>().ReverseMap();
            CreateMap<CreateOrEditPhongDto, PhongDto>().ReverseMap();
        }
    }
}
