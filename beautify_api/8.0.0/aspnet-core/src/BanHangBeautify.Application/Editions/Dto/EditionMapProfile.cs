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
            CreateMap<Edition, EditionListDto>().ReverseMap();
            CreateMap<Edition, CreateEditionDto>().ReverseMap();
            CreateMap<Edition, EditionEditDto>().ReverseMap();
            CreateMap<Feature, FlatFeatureDto>().ReverseMap();
        }
    }
}
