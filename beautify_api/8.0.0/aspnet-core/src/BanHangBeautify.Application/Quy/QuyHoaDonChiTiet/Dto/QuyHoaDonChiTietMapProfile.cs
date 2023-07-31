using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto
{
    public class QuyHoaDonChiTietMapProfile : Profile
    {
        public QuyHoaDonChiTietMapProfile()
        {
            //CreateMap<CreateOrEditQuyHoaDonCTDto, QuyHoaDon_ChiTiet>().ReverseMap();
            CreateMap<QuyHoaDon_ChiTiet, QuyHoaDonChiTietDto>().ReverseMap();
            //CreateMap<CreateOrEditQuyHoaDonCTDto, QuyHoaDonChiTietDto>().ReverseMap();
        }
    }
}
