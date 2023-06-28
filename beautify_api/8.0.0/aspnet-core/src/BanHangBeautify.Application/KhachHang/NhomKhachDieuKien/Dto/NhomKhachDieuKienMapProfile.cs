using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.NhomKhachDieuKien.Dto
{
    public class NhomKhachDieuKienMapProfile:Profile
    {
        public NhomKhachDieuKienMapProfile()
        {
            CreateMap<CreateOrEditNhomKhachDieuKienDto, DM_NhomKhach_DieuKien>().ReverseMap();
            CreateMap<DM_NhomKhach_DieuKien,NhomKhachDieuKienDto>().ReverseMap();
            CreateMap<NhomKhachDieuKienDto, CreateOrEditNhomKhachDieuKienDto>().ReverseMap();
        }
    }
}
