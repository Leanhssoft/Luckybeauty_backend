using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec.Dto
{
    public class LichLamViecMapProfile: Profile
    {
        public LichLamViecMapProfile()
        {
            CreateMap<CreateOrEditLichLamViecDto,NS_LichLamViec>().ReverseMap();
            CreateMap<LichLamViecDto, NS_LichLamViec>().ReverseMap();
            CreateMap<CreateOrEditLichLamViecDto,LichLamViecDto>().ReverseMap();
        }
    }
}
