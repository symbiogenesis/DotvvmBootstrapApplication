using System.IO;
using DotVVM.Framework.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace RingDownConsole
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                BuildWebHost(args).Run();
            }
            catch (DotvvmInterruptRequestExecutionException e)
            {
                Log.Information(e.Message);
            }
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseSerilog()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .Build();
    }
}
