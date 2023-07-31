using System.Globalization;

namespace BanHangBeautify.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}
