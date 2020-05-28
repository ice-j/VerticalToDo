using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace VerticalToDo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)

               .AddJsonFile("appsettings.json", false, true)
               .AddJsonFile($"appsettings.{environment}.json", true, true);
            return Host.CreateDefaultBuilder(args)
                    .UseLamar()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseConfiguration(configBuilder.Build())
                    .UseStartup<Startup>();
                });
        }
    }
}
