using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class DichVuNhanVienMapProfile : Profile
    {
        public DichVuNhanVienMapProfile()
        {
            CreateMap<DichVuNhanVienDto, DichVu_NhanVien>().ReverseMap();
            CreateMap<CreateOrUpdateDichVuNhanVienDto, DichVu_NhanVien>().ReverseMap();
        }
    }
}
