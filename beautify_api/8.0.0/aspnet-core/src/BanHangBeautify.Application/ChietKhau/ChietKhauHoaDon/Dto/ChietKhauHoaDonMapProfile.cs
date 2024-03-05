using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto
{
    public class ChietKhauHoaDonMapProfile : Profile
    {
        public ChietKhauHoaDonMapProfile()
        {
            CreateMap<CreateOrEditChietKhauHDDto, NS_ChietKhauHoaDon>().ReverseMap();
            CreateMap<ChietKhauHoaDonDto, NS_ChietKhauHoaDon>().ReverseMap();
            CreateMap<ChietKhauHoaDonDto, CreateOrEditChietKhauHDDto>().ReverseMap();
        }
    }
}
