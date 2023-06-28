using AutoMapper;
using BanHangBeautify.Entities;

namespace BanHangBeautify.HangHoa.DonViQuiDoi.Dto
{
    public class DonViQuiDoiMapProfile : Profile
    {
        public DonViQuiDoiMapProfile()
        {
            CreateMap<DM_DonViQuiDoi, DonViQuiDoiDto>().ReverseMap();
            CreateMap<DM_DonViQuiDoi, CreateOrEditDonViQuiDoiDto>().ReverseMap();
        }
    }
}
