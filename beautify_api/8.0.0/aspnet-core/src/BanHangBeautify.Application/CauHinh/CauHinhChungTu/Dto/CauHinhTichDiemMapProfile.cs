using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.CauHinh.CauHinhChungTu.Dto
{
    public class CauHinhTichDiemMapProfile:Profile
    {
        public CauHinhTichDiemMapProfile()
        {
            CreateMap<CreateOrEditCauHinhChungTuDto, HT_CauHinh_ChungTu>().ReverseMap();
            CreateMap<CauHinhChungTuDto, HT_CauHinh_ChungTu>().ReverseMap();
            CreateMap<CreateOrEditCauHinhChungTuDto,CauHinhChungTuDto>().ReverseMap();
        }
    }
}
