using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhuyenMai.KhuyenMaiChiTiet.Dto
{
    public class KhuyenMaiChiTietMapProfile: Profile
    {
        public KhuyenMaiChiTietMapProfile()
        {
            CreateMap<KhuyenMaiChiTietDto,DM_KhuyenMai_ChiTiet>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiChiTietDto, DM_KhuyenMai_ChiTiet>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiChiTietDto,KhuyenMaiChiTietDto>().ReverseMap();
        }
    }
}
