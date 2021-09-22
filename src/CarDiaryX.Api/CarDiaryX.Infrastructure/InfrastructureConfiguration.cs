using CarDiaryX.Application.Common;
using CarDiaryX.Application.Features.V1.Identity;
using CarDiaryX.Application.Features.V1.Trips;
using CarDiaryX.Application.Features.V1.Vehicles;
using CarDiaryX.Application.Features.V1.VehicleServices;
using CarDiaryX.Infrastructure.Common;
using CarDiaryX.Infrastructure.Common.Extensions;
using CarDiaryX.Infrastructure.Common.Persistence;
using CarDiaryX.Infrastructure.Identity;
using CarDiaryX.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CarDiaryX.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDatabase(configuration)
                .AddIdentity(configuration)
                .AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDbContext<CarDiaryXDbContext>(options => 
                {
                    options
                        .UseSqlServer(
                            configuration.GetDefaultConnectionString(),
                            sqlServer => sqlServer
                                .MigrationsAssembly(typeof(CarDiaryXDbContext).Assembly.FullName)
                                .EnableRetryOnFailure(
                                    maxRetryCount: 5,
                                    maxRetryDelay: TimeSpan.FromSeconds(10),
                                    errorNumbersToAdd: null
                                ));

                    var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

                    if (isDevelopment)
                    {
                        options.EnableSensitiveDataLogging();
                    }
                });

        private static IServiceCollection AddIdentity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<CarDiaryXDbContext>();

            var secret = configuration
                .GetSection(nameof(ApplicationSettings))
                .GetValue<string>(nameof(ApplicationSettings.Secret));

            var key = Encoding.ASCII.GetBytes(secret);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false; // TODO
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<IIdentity, IdentityService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGeneratorService>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IRegistrationNumberRepository, RegistrationNumberRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IVehicleServicesRepository, VehicleServicesRepository>();

            return services;
        }
    }
}
