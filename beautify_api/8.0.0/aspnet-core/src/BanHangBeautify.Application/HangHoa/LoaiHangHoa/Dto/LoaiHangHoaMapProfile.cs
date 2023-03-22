using AutoMapper;
using BanHangBeautify.Data.Entities;

namespace BanHangBeautify.HangHoa.LoaiHangHoa.Dto
{
    public class LoaiHangHoaMapProfile : Profile
    {
        public LoaiHangHoaMapProfile()
        {
            CreateMap<DM_LoaiHangHoa, LoaiHangHoaDto>().ReverseMap();
            CreateMap<DM_LoaiHangHoa, CreateOrEditLoaiHangHoaDto>().ReverseMap();
        }
    }
}
