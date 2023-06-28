using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.CauHinh.CauHinhTichDiem.Dto
{
    public class CauHinhTichDiemMapProfile:Profile
    {
        public CauHinhTichDiemMapProfile()
        {
            CreateMap<CreateOrEditCauHinhTichDiemDto,HT_CauHinh_TichDiem>().ReverseMap();
            CreateMap<CauHinhTichDiemDto, HT_CauHinh_TichDiem>().ReverseMap();
            CreateMap<CauHinhTichDiemDto,CreateOrEditCauHinhTichDiemDto>().ReverseMap();
        }
    }
}
