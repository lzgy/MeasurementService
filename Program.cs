using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MeasurementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex,"A Host leállt!");
            }
            finally 
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);
            
            //host.UseWindowsService();
            
            host.ConfigureAppConfiguration(
                  (hostContext, config) =>
                  {
                      config.SetBasePath(Directory.GetCurrentDirectory());
                      config.AddJsonFile("appsettings.json", false, true);
                      config.AddCommandLine(args);
                  }
            );
            
            host.ConfigureLogging(
                  loggingBuilder =>
                  {
                      var configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build();
                      var logger = new LoggerConfiguration()
                      .MinimumLevel.Debug()
                      .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                      //.ReadFrom.Settings((Serilog.Configuration.ILoggerSettings)configuration)
                      //.ReadFrom.Configuration(configuration)
                      .CreateLogger();
                      loggingBuilder.AddSerilog(logger, dispose: true);
                  }
            );
           
            host.ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
                //services.AddApplicationInsightsTelemetryWorkerService();
            });
            return host;
        }
    }
}
