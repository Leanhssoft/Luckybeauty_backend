using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto
{
    public class TaiKhoanNganHangMapProfile : Profile
    {
        public TaiKhoanNganHangMapProfile()
        {
            CreateMap<CreateOrEditTaiKhoanNganHangDto, DM_TaiKhoanNganHang>().ReverseMap();
            CreateMap<CreateOrEditTaiKhoanNganHangDto, TaiKhoanNganHangDto>().ReverseMap();
            CreateMap<TaiKhoanNganHangDto, DM_TaiKhoanNganHang>().ReverseMap();
        }
    }
}
