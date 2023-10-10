using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.MailKit;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Asd.AbpZeroTemplate.DashboardCustomization.Definitions;
using BanHangBeautify.Authorization;

namespace BanHangBeautify
{
    [DependsOn(
        typeof(SPACoreModule),
        typeof(AbpAutoMapperModule),typeof(AbpMailKitModule))]

    public class SPAApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<SPAAuthorizationProvider>();
            Configuration.ReplaceService<IMailKitSmtpBuilder, SPAMailKitSmtpBuilder>();
            IocManager.Register<DashboardConfiguration>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(SPAApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
