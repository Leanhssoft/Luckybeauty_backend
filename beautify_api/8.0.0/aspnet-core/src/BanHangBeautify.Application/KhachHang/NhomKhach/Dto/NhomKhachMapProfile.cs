using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.KhachHang.NhomKhach.Dto
{
    public class NhomKhachMapProfile : Profile
    {
        public NhomKhachMapProfile()
        {
            CreateMap<DM_NhomKhachHang, NhomKhachDto>().ReverseMap();
            CreateMap<DM_NhomKhachHang, CreateOrEditNhomKhachDto>().ReverseMap();
        }
    }
}
