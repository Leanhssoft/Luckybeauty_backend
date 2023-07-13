using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class DichVuNhanVienMapProfile: Profile
    {
        public DichVuNhanVienMapProfile()
        {
            CreateMap<DichVuNhanVienDto, DichVu_NhanVien>().ReverseMap();
            CreateMap<CreateOrUpdateDichVuNhanVienDto, DichVu_NhanVien>().ReverseMap();
        }
    }
}
