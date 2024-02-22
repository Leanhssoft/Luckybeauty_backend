using Abp.Application.Editions;
using Abp.Application.Features;
using AutoMapper;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Editions.Dto
{
    public class EditionMapProfile:Profile
    {
        public EditionMapProfile()
        {
            CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            CreateMap<EditionCreateDto, SubscribableEdition>().ReverseMap();
            CreateMap<SubscribableEdition, EditionListDto>().ReverseMap();
            CreateMap<Edition, EditionEditDto>().ReverseMap();
            CreateMap<Edition, SubscribableEdition>().ReverseMap();
        }
    }
}
