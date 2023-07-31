using Abp.Localization;
using AutoMapper;

namespace BanHangBeautify.Localization.Dto
{
    public class LanguageMapProfile : Profile
    {
        public LanguageMapProfile()
        {
            CreateMap<GetLanguagesOutput, ApplicationLanguageListDto>().ReverseMap();
            CreateMap<ApplicationLanguage, ApplicationLanguageListDto>().ReverseMap();
        }
    }
}
