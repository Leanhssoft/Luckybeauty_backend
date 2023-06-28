using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Localization
{
    public class ApplicationCulturesProvider : IApplicationCulturesProvider, ITransientDependency
    {
        public CultureInfo[] GetAllCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures);
        }
    }
}
