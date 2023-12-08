using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.SMS.Dto
{
    public class HeThongSMSMapProfile : Profile
    {
        public HeThongSMSMapProfile()
        {
            CreateMap<CreateOrEditHeThongSMSDto, HeThong_SMS>().ReverseMap();
            CreateMap<HT_SMSBrandname, BrandnameDto>().ReverseMap();
            CreateMap<HT_SMSBrandname, PageBrandnameDto>().ReverseMap();
            CreateMap<SMS_LichSuNap_ChuyenTien, LichSuNap_ChuyenTienDto>().ReverseMap();
            CreateMap<SMS_Template, MauTinSMSDto>().ReverseMap();
            CreateMap<SMS_CaiDat_NhacNho, CaiDatNhacNhoDto>().ReverseMap();
            CreateMap<CaiDat_NhacNho_ChiTiet, CaiDatNhacNhoChiTietDto>().ReverseMap();
            CreateMap<SMS_NhatKy_GuiTin, NhatKyGuiTinSMSDto>().ReverseMap();
        }
    }
}
