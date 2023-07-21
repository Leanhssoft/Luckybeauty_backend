using AutoMapper;
using BanHangBeautify.Data.Entities;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaMapProfile : Profile
    {
        public HangHoaMapProfile()
        {
            CreateMap<DM_HangHoa, HangHoaDto>().ReverseMap();
            CreateMap<DM_HangHoa, CreateOrEditHangHoaDto>().ReverseMap();
            CreateMap<HangHoaDto, ExcelHangHoaDto>().ReverseMap();
        }
    }
}
