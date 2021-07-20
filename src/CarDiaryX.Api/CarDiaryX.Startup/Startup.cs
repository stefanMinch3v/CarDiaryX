using CarDiaryX.Application;
using CarDiaryX.Application.Features.V1.Vehicles;
using CarDiaryX.Infrastructure;
using CarDiaryX.Infrastructure.Common.Extensions;
using CarDiaryX.Integration;
using CarDiaryX.Web;
using CarDiaryX.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace CarDiaryX.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddApplication(this.Configuration)
                .AddInfrastructure(this.Configuration)
                .AddWebComponents();

            this.AddIntegration(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseValidationExceptionHandler()
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(builder =>
                {
                    builder.WithOrigins("http://localhost:8100");

                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                })
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers())
                .AddSwaggerUI()
                .ApplyMigrations()
                .AddDefaultUser();
        }

        private void AddIntegration(IServiceCollection services)
            => services
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .AddHttpClient<IVehicleHttpService, VehicleHttpService>()
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                        UseCookies = false
                    });
    }
}
