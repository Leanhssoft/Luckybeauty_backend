using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.NganHang.Dto
{
    public class NganHangMapProfile:Profile
    {
        public NganHangMapProfile()
        {
            CreateMap<NganHangDto, DM_NganHang>().ReverseMap();
            CreateMap<CreateOrEditNganHangDto, DM_NganHang>().ReverseMap();
            CreateMap<NganHangDto,CreateOrEditNganHangDto>().ReverseMap();
        }
    }
}
