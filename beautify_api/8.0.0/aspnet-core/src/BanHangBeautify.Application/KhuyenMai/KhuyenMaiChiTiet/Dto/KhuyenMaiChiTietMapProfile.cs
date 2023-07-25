using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.KhuyenMai.KhuyenMaiChiTiet.Dto
{
    public class KhuyenMaiChiTietMapProfile : Profile
    {
        public KhuyenMaiChiTietMapProfile()
        {
            CreateMap<KhuyenMaiChiTietDto, DM_KhuyenMai_ChiTiet>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiChiTietDto, DM_KhuyenMai_ChiTiet>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiChiTietDto, KhuyenMaiChiTietDto>().ReverseMap();
        }
    }
}
