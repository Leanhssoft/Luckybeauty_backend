using AutoMapper;
using BanHangBeautify.Data.Entities;

namespace BanHangBeautify.PhongBan.Dto
{
    public class PhongBanMapProfile : Profile
    {
        public PhongBanMapProfile()
        {
            CreateMap<DM_PhongBan, PhongBanDto>().ReverseMap();
            CreateMap<DM_PhongBan, CreateOrEditPhongBanDto>().ReverseMap();
        }
    }
}
