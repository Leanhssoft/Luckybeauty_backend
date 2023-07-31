using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.AppDanhMuc.MauIn.Dto
{
    public class MauInMapProfile : Profile
    {
        public MauInMapProfile()
        {
            CreateMap<CreateOrEditMauInDto, DM_MauIn>().ReverseMap();
            CreateMap<MauInDto, DM_MauIn>().ReverseMap();
        }
    }
}
