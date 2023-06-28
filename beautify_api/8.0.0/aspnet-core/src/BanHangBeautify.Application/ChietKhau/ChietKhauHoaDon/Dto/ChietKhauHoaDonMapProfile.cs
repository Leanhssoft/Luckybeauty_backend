using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto
{
    public class ChietKhauHoaDonMapProfile:Profile
    {
        public ChietKhauHoaDonMapProfile()
        {
            CreateMap<CreateOrEditChietKhauHDDto, NS_ChietKhauHoaDon>().ReverseMap();
            CreateMap<ChietKhauHoaDonDto, NS_ChietKhauHoaDon>().ReverseMap();
            CreateMap<ChietKhauHoaDonDto,CreateOrEditChietKhauHDDto>().ReverseMap();
        }
    }
}
