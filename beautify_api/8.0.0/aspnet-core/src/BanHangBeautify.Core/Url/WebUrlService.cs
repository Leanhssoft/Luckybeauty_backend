using Abp.Application.Services;
using Abp.Dependency;
using Abp.Extensions;
using BanHangBeautify.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        readonly IConfigurationRoot _appConfiguration;
        
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
            _appConfiguration = appConfigurationAccessor.Configuration;
        }

        public override string WebSiteRootAddressFormatKey => "App:ClientRootAddress";

        public override string ServerRootAddressFormatKey => "App:ServerRootAddress";

        public bool SupportsTenancyNameInUrl => false;

        public List<string> GetRedirectAllowedExternalWebSites()
        {
            var values = _appConfiguration[WebSiteRootAddressFormatKey];
            return values?.Split(',').ToList() ?? new List<string>();
        }

        public string GetServerRootAddress(string tenancyName = null)
        {
            return ReplaceTenancyNameInUrl(_appConfiguration[ServerRootAddressFormatKey], tenancyName);
        }

        public string GetSiteRootAddress(string tenancyName = null)
        {
            return ReplaceTenancyNameInUrl(_appConfiguration[WebSiteRootAddressFormatKey], tenancyName);
        }
        private string ReplaceTenancyNameInUrl(string siteRootFormat, string tenancyName)
        {
            if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
            {
                return siteRootFormat;
            }

            if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
            {
                siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", "Account/ChangePassword");
            }

            if (tenancyName.IsNullOrEmpty())
            {
                return siteRootFormat.Replace(TenancyNamePlaceHolder, "Account/ChangePassword");
            }

            return siteRootFormat.Replace(TenancyNamePlaceHolder, "Account/ChangePassword");
        }
    }
}
