using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto
{
    public class ChietKhauDichVuMapProfile : Profile
    {
        public ChietKhauDichVuMapProfile()
        {
            CreateMap<ChietKhauDichVuDto, NS_ChietKhauDichVu>().ReverseMap();
            CreateMap<CreateOrEditChietKhauDichVuDto, NS_ChietKhauDichVu>().ReverseMap();
            CreateMap<ChietKhauDichVuDto, CreateOrEditChietKhauDichVuDto>().ReverseMap();
        }
    }
}
