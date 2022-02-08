using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Iotvn.Service.Redis
{
    public class ServiceRedis : BackgroundService
    {
        #region [ Init ]

        readonly string ServiceName = nameof(ServiceRedis);
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        readonly IWebHostEnvironment _environment;
        public ServiceRedis(ILoggerFactory loggerFactory,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _configuration = configuration;
            _environment = env;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));
            await Task.Delay(Timeout.Infinite, stoppingToken);
            _logger.LogDebug($"{ServiceName} is stopping.");
        }

        #endregion

        oRedisConfig[] _redisConfigs = new oRedisConfig[] { };
        public oRedisConfig[] getAllConfig() => _redisConfigs;

        public async Task<oRedisConfig[]> loadConfig()
        {
            oRedisConfig[] a = new oRedisConfig[] { };
            string file = _environment.WebRootPath + "\\config\\redis.json";
            if (System.IO.File.Exists(file))
            {
                string json = await System.IO.File.ReadAllTextAsync(file);
                try
                {
                    a = JsonConvert.DeserializeObject<oRedisConfig[]>(json);
                    a = a.GroupBy(x => x.port).Select(x => x.First()).ToArray();
                    lock (_redisConfigs) _redisConfigs = a;
                }
                catch { }
            }
            return a;
        }
    }
}
