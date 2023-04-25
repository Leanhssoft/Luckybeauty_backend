using AutoMapper;
using BanHangBeautify.Data.Entities;

namespace BanHangBeautify.NhanSu.NhanVien.Dto
{
    public class NhanSuMapProfile : Profile
    {
        public NhanSuMapProfile()
        {
            CreateMap<NS_NhanVien, NhanSuDto>().ReverseMap();
            CreateMap<NS_NhanVien, NhanSuItemDto>().ReverseMap();
            CreateMap<NS_NhanVien, CreateOrEditNhanSuDto>().ReverseMap();
        }
    }
}
