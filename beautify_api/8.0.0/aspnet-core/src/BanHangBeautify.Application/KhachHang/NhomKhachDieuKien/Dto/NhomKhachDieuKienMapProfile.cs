using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.KhachHang.NhomKhachDieuKien.Dto
{
    public class NhomKhachDieuKienMapProfile : Profile
    {
        public NhomKhachDieuKienMapProfile()
        {
            CreateMap<CreateOrEditNhomKhachDieuKienDto, DM_NhomKhach_DieuKien>().ReverseMap();
            CreateMap<DM_NhomKhach_DieuKien, NhomKhachDieuKienDto>().ReverseMap();
            CreateMap<NhomKhachDieuKienDto, CreateOrEditNhomKhachDieuKienDto>().ReverseMap();
        }
    }
}
