using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Seed;

namespace BanHangBeautify.EntityFrameworkCore
{
    [DependsOn(
        typeof(SPACoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class SPAEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<SPADbContext>(options =>
              {
                  if (options.ExistingConnection != null)
                  {
                      SPADbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                  }
                  else
                  {
                      SPADbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                  }
              });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SPAEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
