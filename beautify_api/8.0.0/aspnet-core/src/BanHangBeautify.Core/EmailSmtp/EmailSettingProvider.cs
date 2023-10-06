using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.EmailSmtp
{
    public class EmailSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                {
                    new SettingDefinition(
                        "SmtpServerAddress",
                        "127.0.0.1"
                        ),

                    new SettingDefinition(
                        "PassiveUsersCanNotLogin",
                        "true",
                        scopes: SettingScopes.Application | SettingScopes.Tenant
                        ),

                    new SettingDefinition(
                        "SiteColorPreference",
                        "red",
                        scopes: SettingScopes.User,
                        clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                        )

                };
        }
    }
}
