using AutoMapper;
using BanHangBeautify.Entities;
using BanHangBeautify.Zalo.DangKyThanhVien;
using BanHangBeautify.Zalo.KetNoi_XacThuc;
using BanHangBeautify.Zalo.ZaloTemplate;

namespace BanHangBeautify.Zalo
{
    public class ZaloMapProfile: Profile
    {
        public ZaloMapProfile()
        {
            CreateMap<ZaloAuthorization, ZaloAuthorizationDto>().ReverseMap();
            CreateMap<Zalo_KhachHangThanhVien, Zalo_KhachHangThanhVienDto>().ReverseMap();
            CreateMap<Zalo_TemplateDto, Zalo_Template>().ReverseMap();
            CreateMap<Zalo_ElementDto, Zalo_Element>().ReverseMap();
            CreateMap<Zalo_TableDetailDto, Zalo_TableDetail>().ReverseMap();
            CreateMap<Zalo_ButtonDetailDto, Zalo_ButtonDetail>().ReverseMap();
        }
    }
}
