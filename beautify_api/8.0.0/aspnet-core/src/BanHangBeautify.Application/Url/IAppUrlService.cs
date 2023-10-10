using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Url
{
    public interface IAppUrlService
    {
        string CreateEmailActivationUrlFormat(int? tenantId);

        string CreatePasswordResetUrlFormat(int? tenantId);

        string CreateEmailActivationUrlFormat(string tenancyName);

        string CreatePasswordResetUrlFormat(string tenancyName);
    }
}
