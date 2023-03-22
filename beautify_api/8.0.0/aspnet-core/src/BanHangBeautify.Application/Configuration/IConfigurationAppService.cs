using BanHangBeautify.Configuration.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
