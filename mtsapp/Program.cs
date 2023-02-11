using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace mtsapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Money Transfer System API";
			string workingDirectory = Environment.CurrentDirectory;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Error)
                .WriteTo.File(workingDirectory+"/logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: null, retainedFileCountLimit: null,
                    shared: true
                    , outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.Console(theme: AnsiConsoleTheme.Grayscale)
                .CreateLogger();
            try
            {
                Log.Information(" Application  Starting.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
            .ConfigureWebHostDefaults((webBuilder) =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    // Set properties and call methods on options
                })
                .UseStartup<Startup>();
            }
            );
    }
}