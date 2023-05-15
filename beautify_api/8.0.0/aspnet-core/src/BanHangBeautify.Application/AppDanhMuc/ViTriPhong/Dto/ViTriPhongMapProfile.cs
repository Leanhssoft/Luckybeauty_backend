using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.ViTriPhong.Dto
{
    public class ViTriPhongMapProfile:Profile
    {
        public ViTriPhongMapProfile()
        {
            CreateMap<CreateOrEditViTriPhongDto,DM_ViTriPhong>().ReverseMap();
            CreateMap<CreateOrEditViTriPhongDto,ViTriPhongDto>().ReverseMap();
            CreateMap<DM_ViTriPhong,ViTriPhongDto>().ReverseMap();
        }
    }
}
