using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.AppCuaHang.Dto
{
    public class CuaHangMapProfile : Profile
    {
        public CuaHangMapProfile()
        {
            CreateMap<CuaHangDto, HT_CongTy>().ReverseMap();
            CreateMap<HT_CongTy, CreateOrEditCuaHangDto>().ReverseMap();
        }
    }
}
