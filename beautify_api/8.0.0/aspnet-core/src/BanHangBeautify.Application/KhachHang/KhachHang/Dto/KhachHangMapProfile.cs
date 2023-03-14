using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class KhachHangMapProfile : Profile
    {
        public KhachHangMapProfile()
        {
            CreateMap<DM_KhachHang, CreateOrEditKhachHangDto>().ReverseMap();
            CreateMap<DM_KhachHang, KhachHangDto>().ReverseMap();
        }
    }
}
