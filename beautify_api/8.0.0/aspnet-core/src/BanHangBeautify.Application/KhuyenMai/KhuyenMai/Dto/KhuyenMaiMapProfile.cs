using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.KhuyenMai.KhuyenMai.Dto
{
    public class KhuyenMaiMapProfile : Profile
    {
        public KhuyenMaiMapProfile()
        {
            CreateMap<KhuyenMaiDto, DM_KhuyenMai>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiDto, DM_KhuyenMai>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiDto, KhuyenMaiDto>().ReverseMap();
        }
    }
}
