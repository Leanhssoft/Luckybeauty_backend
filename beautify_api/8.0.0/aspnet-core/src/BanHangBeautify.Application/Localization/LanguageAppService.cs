using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Localization;
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
    }
    public class GetLanguagesOutput : ListResultDto<ApplicationLanguageListDto>
    {
        public string DefaultLanguageName { get; set; }

        public GetLanguagesOutput()
        {

        }

        public GetLanguagesOutput(IReadOnlyList<ApplicationLanguageListDto> items, string defaultLanguageName)
            : base(items)
        {
            DefaultLanguageName = defaultLanguageName;
        }
    }
}
