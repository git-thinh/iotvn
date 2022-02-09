using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Iotvn.Service.Image
{
    [Route("api/[controller]")]
    public class VectorController : Controller
    {
        static IDatabase _redisDB = null;
        string _redisCnStr = "10.1.1.117:1000,allowAdmin=true,abortConnect=false,defaultDatabase=6,syncTimeout=5000";

        const int __DPI = 70;

        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        readonly IWebHostEnvironment _environment;
        readonly ServiceImage _gs;
        public VectorController(ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            ServiceImage gs)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _configuration = configuration;
            _environment = environment;
            _gs = gs;
        }

        [HttpPost("size")]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<oSize> getSize(IFormFile file, int dpi = __DPI)
        {
            var file2 = HttpContext.Request.Form.Files;
            if (file == null && file2 != null && file2.Count > 0) file = file2[0];
            if (file != null)
            {                
                var o = await _gs.vectorToSize_pageFirst(file.OpenReadStream(), dpi);
                return new oSize { Width = o.Item1, Height = o.Item2 };
            }
            return new oSize();
        }

        [HttpPost("to-png")]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> toPNG(IFormFile file, int dpi = __DPI, int quality = 80)
        {
            var file2 = HttpContext.Request.Form.Files;
            if (file == null && file2 != null && file2.Count > 0) file = file2[0];
            if (file != null)
            {
                //if (file.ContentType != "application/postscript") return new Exception("File must be type *.pdf | *.ai | *.eps");

                var buf = await _gs.vectorToPNG_pageFirst(file.OpenReadStream(), dpi, quality, file.FileName);
                if (buf != null)
                    return File(buf, "image/png");
            }
            return NotFound();
        }

        [HttpPost("to-webp")]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> toWebP(IFormFile file, int quality = 80)
        {
            var file2 = HttpContext.Request.Form.Files;
            if (file == null && file2 != null && file2.Count > 0) file = file2[0];
            if (file != null)
            {
                var buf = await _gs.imageToWebP(file.OpenReadStream(), quality, file.FileName);
                if (buf != null)
                    return File(buf, "image/webp");
            }
            return NotFound();
        }

        [HttpPost("to-pdf")]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> toPDF(IFormFile file, int dpi = __DPI)
        {
            var file2 = HttpContext.Request.Form.Files;
            if (file == null && file2 != null && file2.Count > 0) file = file2[0];
            if (file != null)
            {
                //if (file.ContentType != "application/postscript") return new Exception("File must be type *.ai | *.eps");

                var buf = await _gs.vectorToPDF(file.OpenReadStream(), dpi, file.FileName);
                if (buf != null)
                    return File(buf, "application/pdf");
            }
            return NotFound();
        }
    }

    public class oSize
    {
        public int Width { set; get; }
        public int Height { set; get; }
    }
}
