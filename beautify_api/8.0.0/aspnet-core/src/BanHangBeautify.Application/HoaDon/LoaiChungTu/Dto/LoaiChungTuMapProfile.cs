using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.HoaDon.ChungTu.Dto
{
    public class LoaiChungTuMapProfile : Profile
    {
        public LoaiChungTuMapProfile()
        {
            CreateMap<CreateOrEditLoaiChungTuDto, DM_LoaiChungTu>().ReverseMap();
            CreateMap<LoaiChungTuDto, DM_LoaiChungTu>().ReverseMap();
            CreateMap<CreateOrEditLoaiChungTuDto, LoaiChungTuDto>().ReverseMap();
        }
    }
}
