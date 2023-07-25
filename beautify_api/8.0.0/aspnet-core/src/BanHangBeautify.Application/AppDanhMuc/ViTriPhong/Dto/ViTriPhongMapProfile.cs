using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.ViTriPhong.Dto
{
    public class ViTriPhongMapProfile : Profile
    {
        public ViTriPhongMapProfile()
        {
            CreateMap<CreateOrEditViTriPhongDto, DM_ViTriPhong>().ReverseMap();
            CreateMap<CreateOrEditViTriPhongDto, ViTriPhongDto>().ReverseMap();
            CreateMap<DM_ViTriPhong, ViTriPhongDto>().ReverseMap();
        }
    }
}
