using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Dto
{
    public class NhanVienThucHienMapProfile:Profile
    {
        public NhanVienThucHienMapProfile()
        {
            CreateMap<CreateOrEditNhanVienThucHienDto, BH_NhanVienThucHien>().ReverseMap();
            CreateMap<NhanVienThucHienDto, BH_NhanVienThucHien>().ReverseMap();
            CreateMap<NhanVienThucHienDto,CreateOrEditNhanVienThucHienDto>().ReverseMap();  
        }
    }
}
