using AutoMapper;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using BanHangBeautify.Zalo.DangKyThanhVien;
using BanHangBeautify.Zalo.KetNoi_XacThuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo
{
    public class ZaloMapProfile : Profile
    {
        public ZaloMapProfile()
        {
            CreateMap<ZaloAuthorization, ZaloAuthorizationDto>().ReverseMap();
            CreateMap<Zalo_KhachHangThanhVien, Zalo_KhachHangThanhVienDto>().ReverseMap();
        }
    }
}
