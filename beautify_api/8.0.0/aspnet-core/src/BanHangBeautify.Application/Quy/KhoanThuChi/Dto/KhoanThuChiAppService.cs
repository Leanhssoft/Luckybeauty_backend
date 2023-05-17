using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.KhoanThuChi.Dto
{
    public class KhoanThuChiAppService: Profile
    {
        public KhoanThuChiAppService()
        {
            CreateMap<CreateOrEditKhoanThuChiDto,DM_KhoanThuChi>().ReverseMap();
            CreateMap<KhoanThuChiDto,DM_KhoanThuChi>().ReverseMap();
            CreateMap<CreateOrEditKhoanThuChiDto,KhoanThuChiDto>().ReverseMap();
        }
    }
}
