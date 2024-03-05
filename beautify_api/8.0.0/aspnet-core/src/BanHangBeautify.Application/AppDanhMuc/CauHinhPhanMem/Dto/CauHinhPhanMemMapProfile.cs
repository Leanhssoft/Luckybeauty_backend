using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.CauHinhPhanMem.Dto
{
    public class CauHinhPhanMemMapProfile : Profile
    {
        public CauHinhPhanMemMapProfile()
        {
            CreateMap<CauHinhPhanMemDto, HT_CauHinhPhanMem>().ReverseMap();
            CreateMap<CauHinhPhanMemDto, CreateOrEditCauHinhDto>().ReverseMap();
            CreateMap<CreateOrEditCauHinhDto, HT_CauHinhPhanMem>().ReverseMap();
        }
    }
}
