using Abp.AspNetCore.Dependency;
using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace BanHangBeautify.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(opt =>
                {
                    opt.AddServerHeader = false;
                    opt.Limits.MaxRequestLineSize = 16 * 1024;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIIS()
                .UseIISIntegration()
                .UseStartup<Startup>();

                })
                .UseCastleWindsor(IocManager.Instance.IocContainer);
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    return new WebHostBuilder()
        //        .UseKestrel(opt =>
        //        {
        //            opt.AddServerHeader = false;
        //            opt.Limits.MaxRequestLineSize = 16 * 1024;
        //        })
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseIIS()
        //        .UseIISIntegration()
        //        .UseStartup<Startup>();
        //}
    }
}
