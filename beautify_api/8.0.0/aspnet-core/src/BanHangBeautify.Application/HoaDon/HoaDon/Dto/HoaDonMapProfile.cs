using AutoMapper;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class HoaDonMapProfile : Profile
    {
        public HoaDonMapProfile()
        {
            CreateMap<BH_HoaDon, CreateHoaDonDto>().ReverseMap();
            CreateMap<BH_HoaDon_ChiTiet, HoaDonChiTietDto>().ReverseMap();
        }
    }
}
