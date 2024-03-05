using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.LichLamViec_Ca.Dto
{
    public class LichLamViecCaMapProfile : Profile
    {
        public LichLamViecCaMapProfile()
        {
            CreateMap<CreateOrEditLichLamViecCaDto, NS_LichLamViec_Ca>().ReverseMap();
            CreateMap<NS_LichLamViec_Ca, LichLamViecCaDto>().ReverseMap();
            CreateMap<CreateOrEditLichLamViecCaDto, LichLamViecCaDto>().ReverseMap();
        }
    }
}
