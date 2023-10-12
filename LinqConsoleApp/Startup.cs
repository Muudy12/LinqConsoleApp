using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqConsoleApp
{
    public class Startup
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config.Build())
                .CreateLogger();

            try
            {
                Log.Information("Getting started...");
                var host = CreateHostBuilder(args).Build();
                using IServiceScope scope = host.Services.CreateScope();
                scope.ServiceProvider.GetRequiredService<IProgram>().Run();

            }
            catch (Exception e)
            {
                Log.Fatal(e, "A fatal error occured.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IProgram, Program>();
                })
                .UseSerilog();
        }
    }
}
