using CarDiaryX.Application;
using CarDiaryX.Infrastructure;
using CarDiaryX.Infrastructure.Common.Extensions;
using CarDiaryX.Web;
using CarDiaryX.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            => services
                .AddApplication(this.Configuration)
                .AddInfrastructure(this.Configuration)
                .AddWebComponents();

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
    }
}
