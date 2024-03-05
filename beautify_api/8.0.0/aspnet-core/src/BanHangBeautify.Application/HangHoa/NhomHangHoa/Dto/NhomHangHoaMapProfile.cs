using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.HangHoa.NhomHangHoa.Dto
{
    internal class NhomHangHoaMapProfile : Profile
    {
        public NhomHangHoaMapProfile()
        {
            CreateMap<DM_NhomHangHoa, NhomHangHoaDto>().ReverseMap();
        }
    }
}
