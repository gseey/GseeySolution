using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Gseey.Apis.Weixin
{

#pragma warning disable CS1591
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://*:6000")
            .UseStartup<Startup>()
            ;
    }
#pragma warning restore CS1591
}
