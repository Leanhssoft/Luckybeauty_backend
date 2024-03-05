using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.LichLamViec.Dto
{
    public class LichLamViecMapProfile : Profile
    {
        public LichLamViecMapProfile()
        {
            CreateMap<CreateOrEditLichLamViecDto, NS_LichLamViec>().ReverseMap();
            CreateMap<LichLamViecDto, NS_LichLamViec>().ReverseMap();
            CreateMap<CreateOrEditLichLamViecDto, LichLamViecDto>().ReverseMap();
        }
    }
}
