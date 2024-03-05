using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.NgayNghiLe.Dto
{
    public class NgayNghiLeMapProfile : Profile
    {
        public NgayNghiLeMapProfile()
        {
            CreateMap<DM_NgayNghiLe, NgayNghiLeDto>().ReverseMap();
            CreateMap<DM_NgayNghiLe, CreateOrEditNgayNghiLeDto>().ReverseMap();
        }
    }
}
