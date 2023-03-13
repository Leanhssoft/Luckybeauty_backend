using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using BanHangBeautify.Configuration.Dto;

namespace BanHangBeautify.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : SPAAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
