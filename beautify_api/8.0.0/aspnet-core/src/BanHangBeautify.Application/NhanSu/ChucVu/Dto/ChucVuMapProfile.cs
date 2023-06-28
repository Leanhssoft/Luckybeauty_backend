using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.ChucVu.Dto
{
    public class ChucVuMapProfile : Profile
    {
        public ChucVuMapProfile()
        {
            CreateMap<NS_ChucVu, ChucVuDto>().ReverseMap();
            CreateMap<NS_ChucVu, CreateOrEditChucVuDto>().ReverseMap();
        }
    }
}
