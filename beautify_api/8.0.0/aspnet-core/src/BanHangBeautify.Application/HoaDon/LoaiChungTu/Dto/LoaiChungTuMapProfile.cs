using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.ChungTu.Dto
{
    public class LoaiChungTuMapProfile:Profile
    {
        public LoaiChungTuMapProfile()
        {
            CreateMap<CreateOrEditLoaiChungTuDto,DM_LoaiChungTu>().ReverseMap();
            CreateMap<LoaiChungTuDto,DM_LoaiChungTu>().ReverseMap();
            CreateMap<CreateOrEditLoaiChungTuDto,LoaiChungTuDto>().ReverseMap();
        }
    }
}
