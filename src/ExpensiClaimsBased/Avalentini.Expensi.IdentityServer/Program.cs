using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Avalentini.Expensi.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Expensi Identity Server";

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
