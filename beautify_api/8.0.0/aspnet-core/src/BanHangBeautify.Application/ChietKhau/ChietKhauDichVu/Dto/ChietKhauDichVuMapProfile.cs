using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto
{
    public class ChietKhauDichVuMapProfile:Profile
    {
        public ChietKhauDichVuMapProfile()
        {
            CreateMap<ChietKhauDichVuDto, NS_ChietKhauDichVu>().ReverseMap();
            CreateMap<CreateOrEditChietKhauDichVuDto, NS_ChietKhauDichVu>().ReverseMap();
            CreateMap<ChietKhauDichVuDto,CreateOrEditChietKhauDichVuDto>().ReverseMap();
        }
    }
}
