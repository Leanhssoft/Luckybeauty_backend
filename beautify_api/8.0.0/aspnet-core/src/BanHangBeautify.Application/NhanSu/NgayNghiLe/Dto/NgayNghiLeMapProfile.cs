using AutoMapper;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.ChucVu.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NgayNghiLe.Dto
{
    public class NgayNghiLeMapProfile : Profile
    {
        public NgayNghiLeMapProfile()
        {
            CreateMap<DM_NgayNghiLe, NgayNghiLeDto>().ReverseMap();
            CreateMap<DM_NgayNghiLe, CreateOrEditNgayNghiLeDto>().ReverseMap();
        }
    }
}
