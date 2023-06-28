using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.Phong.Dto
{
    public class PhongMapProfile: Profile
    {
        public PhongMapProfile()
        {
            CreateMap<PhongDto, DM_Phong>().ReverseMap();
            CreateMap<CreateOrEditPhongDto, DM_Phong>().ReverseMap();
            CreateMap<CreateOrEditPhongDto,PhongDto>().ReverseMap();
        }
    }
}
