using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Dto
{
    public class NhanVienThucHienMapProfile : Profile
    {
        public NhanVienThucHienMapProfile()
        {
            CreateMap<CreateOrEditNhanVienThucHienDto, BH_NhanVienThucHien>().ReverseMap();
            CreateMap<NhanVienThucHienDto, BH_NhanVienThucHien>().ReverseMap();
            CreateMap<NhanVienThucHienDto, CreateOrEditNhanVienThucHienDto>().ReverseMap();
        }
    }
}
