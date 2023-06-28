using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.CauHinhPhanMem.Dto
{
    public class CauHinhPhanMemMapProfile: Profile
    {
        public CauHinhPhanMemMapProfile()
        {
            CreateMap<CauHinhPhanMemDto, HT_CauHinhPhanMem>().ReverseMap();
            CreateMap<CauHinhPhanMemDto,CreateOrEditCauHinhDto>().ReverseMap();
            CreateMap<CreateOrEditCauHinhDto, HT_CauHinhPhanMem>().ReverseMap();
        }
    }
}
