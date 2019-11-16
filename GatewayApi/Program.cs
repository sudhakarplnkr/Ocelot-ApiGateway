namespace GatewayApi
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });
            builder.UseKestrel()
            .UseStartup<Startup>()
                   .UseContentRoot(Directory.GetCurrentDirectory());

            var host = builder.Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                           .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                           .AddJsonFile("ocelot.json")
                           .AddEnvironmentVariables();
               })
           .ConfigureServices(s =>
           {
               s.AddOcelot();
               s.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           })
            .Configure(a =>
            {
                a.UseOcelot().Wait();
            });
    }
}
