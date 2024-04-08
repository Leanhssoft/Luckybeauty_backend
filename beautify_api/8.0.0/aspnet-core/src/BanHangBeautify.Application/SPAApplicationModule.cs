using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.MailKit;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Asd.AbpZeroTemplate.DashboardCustomization.Definitions;
using BanHangBeautify.AppWebhook;
using BanHangBeautify.Authorization;
using BanHangBeautify.BackgroundWorker;

namespace BanHangBeautify
{
    [DependsOn(
        typeof(SPACoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpMailKitModule)
    )]

    public class SPAApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<SPAAuthorizationProvider>();
            Configuration.ReplaceService<IMailKitSmtpBuilder, SPAMailKitSmtpBuilder>();
            IocManager.Register<DashboardConfiguration>();
            Configuration.Webhooks.Providers.Add<ZaloHookProvider>();
            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override async void Initialize()
        {
            var thisAssembly = typeof(SPAApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }

        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SendEmailSMSAutoWorker>());
        }
    }
}
