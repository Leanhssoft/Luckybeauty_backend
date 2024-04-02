using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BanHangBeautify.Application.Shared
{
    [DependsOn(typeof(SPAApplicationSharedModule))]
    public class SPAApplicationSharedModule:AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SPAApplicationSharedModule).GetAssembly());
        }
    }
}
