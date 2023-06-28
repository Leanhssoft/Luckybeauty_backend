using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto
{
    public class ChiNhanhMapProfile : Profile
    {
        public ChiNhanhMapProfile()
        {
            CreateMap<DM_ChiNhanh, ChiNhanhDto>().ReverseMap();
            CreateMap<DM_ChiNhanh, CreateChiNhanhDto>().ReverseMap();
            CreateMap<ChiNhanhDto, CreateChiNhanhDto>().ReverseMap();
        }
    }
}
