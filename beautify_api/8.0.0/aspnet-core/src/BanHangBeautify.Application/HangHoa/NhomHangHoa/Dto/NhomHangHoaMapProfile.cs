using AutoMapper;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.NhomHangHoa.Dto
{
    internal class NhomHangHoaMapProfile: Profile
    {
        public NhomHangHoaMapProfile()
        {
            CreateMap<DM_NhomHangHoa, NhomHangHoaDto>().ReverseMap();
        }
    }
}
