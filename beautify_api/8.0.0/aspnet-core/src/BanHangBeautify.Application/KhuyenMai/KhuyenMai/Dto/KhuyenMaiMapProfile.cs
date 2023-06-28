using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhuyenMai.KhuyenMai.Dto
{
    public class KhuyenMaiMapProfile:Profile
    {
        public KhuyenMaiMapProfile()
        {
            CreateMap<KhuyenMaiDto, DM_KhuyenMai>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiDto, DM_KhuyenMai>().ReverseMap();
            CreateMap<CreateOrEditKhuyenMaiDto, KhuyenMaiDto>().ReverseMap();
        }
    }
}
