using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.AnhLieuTrinh
{
    public class AnhLieuTrinhMapProfile : Profile
    {
        public AnhLieuTrinhMapProfile()
        {
            CreateMap<KhachHang_Anh_LieuTrinh, AnhLieuTrinhDto>().ReverseMap();
            CreateMap<KhachHang_Anh_LieuTrinh_ChiTiet, AnhLieuTrinh_ChiTietDto>().ReverseMap();
        }
    }
}
