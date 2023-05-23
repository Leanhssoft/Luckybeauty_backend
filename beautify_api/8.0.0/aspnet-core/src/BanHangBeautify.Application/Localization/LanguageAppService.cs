using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Localization;
using BanHangBeautify.Localization.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Localization
{
    public class LanguageAppService:SPAAppServiceBase
    {
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IApplicationLanguageTextManager _applicationLanguageTextManager;
        private readonly IRepository<ApplicationLanguage> _languageRepository;
        private readonly IApplicationCulturesProvider _applicationCulturesProvider;
        public LanguageAppService(
            IApplicationLanguageManager applicationLanguageManager,
            IApplicationLanguageTextManager applicationLanguageTextManager,
            IRepository<ApplicationLanguage> languageRepository,
            IApplicationCulturesProvider applicationCulturesProvider)
        {
            _applicationLanguageManager = applicationLanguageManager;
            _languageRepository = languageRepository;
            _applicationLanguageTextManager = applicationLanguageTextManager;
            _applicationCulturesProvider = applicationCulturesProvider;
        }
        public async Task<GetLanguagesOutput> GetLanguages()
        {
            var languages =
                (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId)).OrderBy(l => l.DisplayName);
            var defaultLanguage = await _applicationLanguageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);

            return new GetLanguagesOutput(
                ObjectMapper.Map<List<ApplicationLanguageListDto>>(languages),
                defaultLanguage?.Name
            );
        }
        public async Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input)
        {
            ApplicationLanguage language = null;
            if (input.Id.HasValue)
            {
                language = await _languageRepository.GetAsync(input.Id.Value);
            }

            var output = new GetLanguageForEditOutput();

            //Language
            output.Language = language != null
                ? ObjectMapper.Map<ApplicationLanguageEditDto>(language)
                : new ApplicationLanguageEditDto();

            //Language names
            output.LanguageNames = _applicationCulturesProvider
                .GetAllCultures()
                .Select(c => new ComboboxItemDto(c.Name, c.EnglishName + " (" + c.Name + ")")
                { IsSelected = output.Language.Name == c.Name })
                .ToList();

            //Flags
            output.Flags = FamFamFamFlagsHelper
                .FlagClassNames
                .OrderBy(f => f)
                .Select(f => new ComboboxItemDto(f, FamFamFamFlagsHelper.GetCountryCode(f))
                { IsSelected = output.Language.Icon == f })
                .ToList();

            return output;
        }
    }
}
