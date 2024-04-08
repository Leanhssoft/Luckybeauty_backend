using AutoMapper;
using BanHangBeautify.Entities;
using BanHangBeautify.MultiTenancy.Dto;

namespace BanHangBeautify.NhatKyHoatDong.Dto
{
    public class NhatKyThaoTacMapProfile : Profile
    {
        public NhatKyThaoTacMapProfile()
        {
            CreateMap<CreateNhatKyThaoTacDto, HT_NhatKyThaoTac>().ReverseMap();
            CreateMap<HT_NhatKyThaoTac, NhatKyThaoTacDto>().ReverseMap();
            CreateMap<HT_NhatKyThaoTac, NhatKyThaoTacItemDto>().ReverseMap();
            CreateMap<CreateNhatKyThaoTacDto, NhatKyThaoTacItemDto>().ReverseMap();
            CreateMap<CreateNhatKyThaoTacDto, NhatKyThaoTacDto>().ReverseMap();
            CreateMap<NhatKyThaoTacDto, NhatKyThaoTacItemDto>().ReverseMap();
            CreateMap<HT_NhatKyThaoTac, TenantHistoryActivityDto>().ReverseMap();
        }
    }
}
