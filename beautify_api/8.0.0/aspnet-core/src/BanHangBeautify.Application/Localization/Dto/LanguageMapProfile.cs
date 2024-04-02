using Abp.Localization;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Localization.Dto
{
    public class LanguageMapProfile : Profile
    {
        public LanguageMapProfile()
        {
            CreateMap<ApplicationLanguageListDto, ApplicationLanguage>().ReverseMap();
        }
    }
}
