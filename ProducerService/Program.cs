using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddSerilog();
                }).UseStartup<Startup>();

            return webHostBuilder.Build();
        }
    }
}
