using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.SMS.Dto
{
    public class HeThongSMSMapProfile:Profile
    {
        public HeThongSMSMapProfile()
        {
            CreateMap<CreateOrEditHeThongSMSDto,HeThong_SMS>().ReverseMap();
            CreateMap<HeThongSMSDto, HeThong_SMS>().ReverseMap();
            CreateMap<CreateOrEditHeThongSMSDto,HeThongSMSDto>().ReverseMap();
        }
    }
}
