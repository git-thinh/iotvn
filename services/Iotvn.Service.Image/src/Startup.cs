using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Iotvn.Service.Image
{
    public class Program { public static void Main(string[] args) => App.Run<Startup>(args); }

    public class Startup
    {
        IConfiguration _configuration { get; }
        IWebHostEnvironment _environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ServiceImage>();
            services.AddSingleton<IHostedService>(p => p.GetService<ServiceImage>());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
            services.AddControllers().AddApplicationPart(this.GetType().Assembly).AddControllersAsServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseStaticFiles();
            //app.UseDirectoryBrowser();

            //--------------------------------------------------------

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TTGD.ServiceJob v1.0"));

            //--------------------------------------------------------

            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    context.Response.ContentType = "text/html";
                    string file = _environment.WebRootPath + "\\api-doc.html";
                    byte[] bs = new byte[] { };
                    if (System.IO.File.Exists(file)) bs = System.IO.File.ReadAllBytes(file);
                    await context.Response.Body.WriteAsync(bs, 0, bs.Length);
                });
            });
        }
    }
}
