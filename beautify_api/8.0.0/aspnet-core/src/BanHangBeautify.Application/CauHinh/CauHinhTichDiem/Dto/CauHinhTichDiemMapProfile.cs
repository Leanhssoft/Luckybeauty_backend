using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.CauHinh.CauHinhTichDiem.Dto
{
    public class CauHinhTichDiemMapProfile : Profile
    {
        public CauHinhTichDiemMapProfile()
        {
            CreateMap<CreateOrEditCauHinhTichDiemDto, HT_CauHinh_TichDiem>().ReverseMap();
            CreateMap<CauHinhTichDiemDto, HT_CauHinh_TichDiem>().ReverseMap();
            CreateMap<CauHinhTichDiemDto, CreateOrEditCauHinhTichDiemDto>().ReverseMap();
        }
    }
}
