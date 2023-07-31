using AutoMapper;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class HoaDonMapProfile : Profile
    {
        public HoaDonMapProfile()
        {
            CreateMap<BH_HoaDon, CreateHoaDonDto>().ReverseMap();
            CreateMap<BH_HoaDon_ChiTiet, HoaDonChiTietDto>().ReverseMap();
        }
    }
}
