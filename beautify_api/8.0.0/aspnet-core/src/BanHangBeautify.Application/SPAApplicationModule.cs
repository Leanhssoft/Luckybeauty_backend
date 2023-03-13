using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BanHangBeautify.Authorization;

namespace BanHangBeautify
{
    [DependsOn(
        typeof(SPACoreModule), 
        typeof(AbpAutoMapperModule))]
    public class SPAApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<SPAAuthorizationProvider>();
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
