using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.KhachHang.LoaiKhach.Dto
{
    public class LoaiKhachMapProfile : Profile
    {
        public LoaiKhachMapProfile()
        {
            CreateMap<DM_LoaiKhach, LoaiKhachDto>().ReverseMap();
            CreateMap<DM_LoaiKhach, CreateOrEditLoaiKhachDto>().ReverseMap();
        }
    }
}
