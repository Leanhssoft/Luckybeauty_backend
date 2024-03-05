using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.Quy.KhoanThuChi.Dto
{
    public class KhoanThuChiAppService : Profile
    {
        public KhoanThuChiAppService()
        {
            CreateMap<CreateOrEditKhoanThuChiDto, DM_KhoanThuChi>().ReverseMap();
            CreateMap<KhoanThuChiDto, DM_KhoanThuChi>().ReverseMap();
            CreateMap<CreateOrEditKhoanThuChiDto, KhoanThuChiDto>().ReverseMap();
        }
    }
}
