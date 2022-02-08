using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using NLog.Web;
//using NLog.Web.AspNetCore;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Iotvn
{
    public static class App
    {
        static IServiceProvider _service = null;
        public static IServiceProvider getService() => _service;
        public static void Run<T>(string[] args)
        {
            //var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                //logger.Debug("App.Run(): Init main...");
                var host = CreateHostBuilder<T>(args).Build();
                _service = host.Services;
                host.Run();
            }
            catch (Exception exception)
            {
                //logger.Error(exception, "App.Run(): Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                //NLog.LogManager.Shutdown();
            }
        }

        static string __MD5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            for (int i = 0; i < bytes.Length; i++)
                hash.Append(bytes[i].ToString("X2"));
            return hash.ToString();
        }

        static IHostBuilder CreateHostBuilder<T>(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                //.UseKestrel(options =>
                //{
                //    options.ListenLocalhost(5000);
                //    options.ListenLocalhost(5001);
                //    options.ListenLocalhost(5002);
                //})
                //.UseIISIntegration()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    string environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string file = Path.Combine(path, "appsettings.json");
                    if (File.Exists(file)) config.AddJsonFile(file, optional: true);

                    file = Path.Combine(path, string.Format("appsettings.{0}.json", environmentName));
                    if (File.Exists(file)) config.AddJsonFile(file, optional: true);

                    //file = Path.Combine(path, string.Format("appsettings.{0}.Redis.json", environmentName));
                    //if (File.Exists(file)) config.AddJsonFile(file, optional: true);

                    //file = Path.Combine(path, string.Format("appsettings.{0}.ConnectionString.json", environmentName));
                    //if (File.Exists(file)) config.AddJsonFile(file, optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Type typeStartup = typeof(T);
                    webBuilder.UseStartup(typeStartup);
                })
                .ConfigureServices(services =>
                {
                    //services.AddHostedService<LifetimeEventsHostedService>();
                    //services.AddHostedService<TimedHostedService>();
                })
                .ConfigureLogging(logging =>
                {
                    //logging.ClearProviders();
                    //logging.SetMinimumLevel(LogLevel.Trace);
                });
                //.UseNLog(); // NLog: Setup NLog for Dependency injection
            return host;
        }
    }
}
