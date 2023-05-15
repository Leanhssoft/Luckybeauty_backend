using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.MauIn.Dto
{
    public class MauInMapProfile:Profile
    {
        public MauInMapProfile()
        {
            CreateMap<CreateOrEditMauInDto, DM_MauIn>().ReverseMap();
            CreateMap<MauInDto, DM_MauIn>().ReverseMap();
        }
    }
}
