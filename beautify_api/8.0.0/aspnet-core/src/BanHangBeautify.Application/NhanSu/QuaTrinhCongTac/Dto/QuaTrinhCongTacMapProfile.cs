using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.NhanSu.QuaTrinhCongTac.Dto
{
    public class QuaTrinhCongTacMapProfile : Profile
    {
        public QuaTrinhCongTacMapProfile()
        {
            CreateMap<NS_QuaTrinh_CongTac, CreateOrEditQuaTrinhConTacDto>().ReverseMap();
            CreateMap<NS_QuaTrinh_CongTac, QuaTrinhConTacDto>().ReverseMap();
        }
    }
}
