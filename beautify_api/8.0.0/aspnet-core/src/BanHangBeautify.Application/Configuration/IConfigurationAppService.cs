using System.Threading.Tasks;
using BanHangBeautify.Configuration.Dto;

namespace BanHangBeautify.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
