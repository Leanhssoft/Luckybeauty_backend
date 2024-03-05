using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.NhanVien_TimeOff.Dto
{
    public class NhanVienTimeOffMapProfile : Profile
    {
        public NhanVienTimeOffMapProfile()
        {
            CreateMap<NhanVienTimeOffDto, NS_NhanVien_TimeOff>().ReverseMap();
            CreateMap<NhanVienTimeOffDto, CreateOrEditNhanVienTimeOffDto>().ReverseMap();
            CreateMap<CreateOrEditNhanVienTimeOffDto, NS_NhanVien_TimeOff>().ReverseMap();
        }
    }
}
