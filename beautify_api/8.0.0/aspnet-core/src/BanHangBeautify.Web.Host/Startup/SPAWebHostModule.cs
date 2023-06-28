using Abp.Modules;
using Abp.Reflection.Extensions;
using BanHangBeautify.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BanHangBeautify.Web.Host.Startup
{
    [DependsOn(
       typeof(SPAWebCoreModule))]
    public class SPAWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public SPAWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SPAWebHostModule).GetAssembly());
        }
    }
}
