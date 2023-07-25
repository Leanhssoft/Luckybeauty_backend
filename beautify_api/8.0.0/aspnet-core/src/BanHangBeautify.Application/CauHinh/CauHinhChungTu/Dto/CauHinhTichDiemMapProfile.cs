using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.CauHinh.CauHinhChungTu.Dto
{
    public class CauHinhTichDiemMapProfile : Profile
    {
        public CauHinhTichDiemMapProfile()
        {
            CreateMap<CreateOrEditCauHinhChungTuDto, HT_CauHinh_ChungTu>().ReverseMap();
            CreateMap<CauHinhChungTuDto, HT_CauHinh_ChungTu>().ReverseMap();
            CreateMap<CreateOrEditCauHinhChungTuDto, CauHinhChungTuDto>().ReverseMap();
        }
    }
}
