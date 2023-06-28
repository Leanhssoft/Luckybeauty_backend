using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.CaLamViec.Dto
{
    public class CaLamViecMapProfile : Profile
    {
        public CaLamViecMapProfile()
        {
            CreateMap<NS_CaLamViec, CaLamViecDto>().ReverseMap();
            CreateMap<NS_CaLamViec, CreateOrEditCaLamViecDto>().ReverseMap();
        }
    }
}
