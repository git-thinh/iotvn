using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Iotvn.Service.Redis
{
    [Route("api/[controller]")]
    public class RedisController : Controller
    {
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        readonly IWebHostEnvironment _environment;
        readonly ServiceRedis _redis;
        public RedisController(ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            ServiceRedis redis)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _configuration = configuration;
            _environment = environment;
            _redis = redis;
        }

        [ResponseCache(Duration = 60)]
        [HttpGet("all")]
        public oRedisConfig[] getAll() => _redis.getAllConfig();

        [ResponseCache(Duration = 60)]
        [HttpGet("re-load")]
        public async Task<oRedisConfig[]> reload() => await _redis.loadConfig();


    }

}
