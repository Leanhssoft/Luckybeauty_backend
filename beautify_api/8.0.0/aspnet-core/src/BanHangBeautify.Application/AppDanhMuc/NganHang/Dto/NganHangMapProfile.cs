using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.NganHang.Dto
{
    public class NganHangMapProfile : Profile
    {
        public NganHangMapProfile()
        {
            CreateMap<NganHangDto, DM_NganHang>().ReverseMap();
            CreateMap<CreateOrEditNganHangDto, DM_NganHang>().ReverseMap();
            CreateMap<NganHangDto, CreateOrEditNganHangDto>().ReverseMap();
        }
    }
}
