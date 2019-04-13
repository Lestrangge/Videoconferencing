﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace VideoconferencingBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            CreateWebHostBuilder(args).UseUrls("https://*:5000").Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
