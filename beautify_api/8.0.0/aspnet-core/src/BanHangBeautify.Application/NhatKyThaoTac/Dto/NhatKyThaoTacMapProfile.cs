using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhatKyHoatDong.Dto
{
    public class NhatKyThaoTacMapProfile: Profile
    {
        public NhatKyThaoTacMapProfile()
        {
            CreateMap<CreateNhatKyThaoTacDto,HT_NhatKyThaoTac>().ReverseMap();
            CreateMap<HT_NhatKyThaoTac,NhatKyThaoTacDto>().ReverseMap();
            CreateMap<HT_NhatKyThaoTac,NhatKyThaoTacItemDto>().ReverseMap();
            CreateMap<CreateNhatKyThaoTacDto,NhatKyThaoTacItemDto>().ReverseMap();
            CreateMap<CreateNhatKyThaoTacDto, NhatKyThaoTacDto>().ReverseMap();
            CreateMap<NhatKyThaoTacDto, NhatKyThaoTacItemDto>().ReverseMap();
        }
    }
}
