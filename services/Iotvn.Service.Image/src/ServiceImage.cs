using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Iotvn.Service.Image
{
    public class ServiceImage : BackgroundService
    {
        #region [ Init ]

        readonly string ServiceName = nameof(ServiceImage);
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        readonly IWebHostEnvironment _environment;
        public ServiceImage(ILoggerFactory loggerFactory,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _configuration = configuration;
            _environment = env;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _init();
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

        const string GS_VERSION_PATH = @"C:\Program Files\gs\gs9.55.0\bin";

        void _init()
        {
            MagickNET.SetGhostscriptDirectory(GS_VERSION_PATH);
            MagickNET.Initialize();
        }

        public async Task<byte[]> imageToWebP(Stream stream, int quality = 0, string name = "")
        {
            try
            {
                using (var image = new MagickImage(stream))
                {
                    var defines = new ImageMagick.Formats.WebPWriteDefines
                    {
                        Lossless = true,
                        Method = 6,
                    };

                    var mi = new MemoryStream();
                    await image.WriteAsync(mi, defines);
                    var bs = mi.ToArray();

                    //await System.IO.File.WriteAllBytesAsync(_environment.WebRootPath + @"\test\" + name + "-.webp", bs);

                    return bs;
                }
            }
            catch (Exception ex)
            {
            }
            return new byte[] { };
        }

        public async Task<Tuple<int, int>> vectorToSize_pageFirst(Stream stream, int density = 0)
        {
            try
            {
                var cf = new MagickReadSettings();
                if (density > 0) cf.Density = new Density(density, density); //DPI
                cf.FrameIndex = 0; // First page
                cf.FrameCount = 1; // Number of pages
                using (var collection = new MagickImageCollection())
                {
                    await collection.ReadAsync(stream, cf);
                    var image = collection[0];
                    return new Tuple<int, int>(image.Width, image.Height);
                }
            }
            catch (Exception ex)
            {
            }
            return new Tuple<int, int>(0, 0);
        }

        public async Task<byte[]> vectorToPNG_pageFirst(Stream stream, int density = 0, int quality = 80, string name = "")
        {
            try
            {
                var cf = new MagickReadSettings();
                if (density > 0) cf.Density = new Density(density, density); //DPI
                cf.ColorSpace = ColorSpace.sRGB; //Setiing for transparent background
                cf.FrameIndex = 0; // First page
                cf.FrameCount = 1; // Number of pages
                using (var collection = new MagickImageCollection())
                {
                    await collection.ReadAsync(stream, cf);
                    var image = collection[0];
                    //collection.Clear(); // Clear the collection

                    image.Format = MagickFormat.Png;
                    image.ColorAlpha(MagickColors.Transparent);
                    image.Settings.Verbose = true;

                    //image.Trim();
                    //image.Sharpen(0,1.0);

                    image.Settings.BackgroundColor = MagickColors.Transparent;
                    image.Quality = quality;

                    //await image.WriteAsync(_environment.WebRootPath + @"\test\" + name + "-.png", MagickFormat.Png);

                    var bs = image.ToByteArray();
                    return bs;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<byte[]> vectorToPDF(Stream stream, int density = 0, string name = "")
        {
            try
            {
                var cf = new MagickReadSettings();
                if (density > 0) cf.Density = new Density(density, density); //DPI
                cf.ColorSpace = ColorSpace.sRGB; //Setiing for transparent background
                using (var image = new MagickImage(stream, cf))
                {
                    image.Format = MagickFormat.Pdf;
                    //image.ColorAlpha(MagickColors.Transparent);
                    //image.Settings.Verbose = true;
                    //image.Settings.BackgroundColor = MagickColors.Transparent;
                    //image.Quality = quality;

                    //await image.WriteAsync(_environment.WebRootPath + @"\test\" + name + "-.pdf", MagickFormat.Pdf);

                    var bs = image.ToByteArray();
                    return bs;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        async Task<List<byte[]>> __pdfToImages(string filePath, int density = 200, int quality = 80)
        {
            List<byte[]> fileList = new List<byte[]>();
            var settings = new MagickReadSettings();
            settings.Density = new Density(density, density);
            using (var images = new MagickImageCollection())
            {
                await images.ReadAsync(filePath, settings);
                int pageCount = images.Count;
                for (int i = 0; i < pageCount; i++)
                {
                    var image = images[i];
                    image.Format = MagickFormat.Png;
                    image.ColorAlpha(MagickColors.White);
                    image.Settings.Verbose = true;
                    //image.Trim();
                    //image.Sharpen(0,1.0);

                    image.Settings.BackgroundColor = MagickColors.White;
                    image.Quality = quality;

                    //string imgPath = $"{fileName}_{i}.png"; //相对路径   
                    //string filename = Path.Combine(outDir, imgPath);                    
                    //await image.WriteAsync(filename);

                    var mi = new MemoryStream();
                    await image.WriteAsync(mi);

                    fileList.Add(mi.ToArray());
                }
            }

            return fileList;
        }

    }
}
