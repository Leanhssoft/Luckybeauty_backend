using Abp.Localization;
using Abp.Modules;
using Abp.Notifications;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Configuration;
using BanHangBeautify.Localization;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.Notifications;
using BanHangBeautify.Timing;

namespace BanHangBeautify
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class SPACoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            SPALocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = SPAConsts.MultiTenancyEnabled;

            //Adding notification providers
            Configuration.Notifications.Providers.Add<AppNotificationProvider>();
            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));

            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = SPAConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = SPAConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SPACoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
