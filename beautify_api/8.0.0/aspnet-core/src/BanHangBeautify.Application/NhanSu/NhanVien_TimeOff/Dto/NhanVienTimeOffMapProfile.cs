using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_TimeOff.Dto
{
    public class NhanVienTimeOffMapProfile: Profile
    {
        public NhanVienTimeOffMapProfile()
        {
            CreateMap<NhanVienTimeOffDto,NS_NhanVien_TimeOff>().ReverseMap();
            CreateMap<NhanVienTimeOffDto,CreateOrEditNhanVienTimeOffDto>().ReverseMap();
            CreateMap<CreateOrEditNhanVienTimeOffDto, NS_NhanVien_TimeOff>().ReverseMap();
        }
    }
}
