using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto
{
    public class TaiKhoanNganHangMapProfile:Profile
    {
        public TaiKhoanNganHangMapProfile()
        {
            CreateMap<CreateOrEditTaiKhoanNganHangDto, DM_TaiKhoanNganHang>().ReverseMap();
            CreateMap<CreateOrEditTaiKhoanNganHangDto,TaiKhoanNganHangDto>().ReverseMap();
            CreateMap<TaiKhoanNganHangDto, DM_TaiKhoanNganHang>().ReverseMap();
        }
    }
}
