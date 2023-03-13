using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SSOFT.SPA.EntityFrameworkCore;
using SSOFT.SPA.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace SSOFT.SPA.Web.Tests
{
    [DependsOn(
        typeof(SPAWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class SPAWebTestModule : AbpModule
    {
        public SPAWebTestModule(SPAEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SPAWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(SPAWebMvcModule).Assembly);
        }
    }
}