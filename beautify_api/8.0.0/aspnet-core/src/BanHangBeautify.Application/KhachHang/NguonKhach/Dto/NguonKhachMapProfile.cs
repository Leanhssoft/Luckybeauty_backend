using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.KhachHang.NguonKhach.Dto
{
    public class NguonKhachMapProfile : Profile
    {
        public NguonKhachMapProfile()
        {
            CreateMap<DM_NguonKhach, CreateOrEditNguonKhachDto>().ReverseMap();
            CreateMap<DM_NguonKhach, NguonKhachDto>().ReverseMap();
        }
    }
}
