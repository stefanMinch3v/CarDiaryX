using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.BackgroundServices;
using CarDiaryX.Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CarDiaryX.Application
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
            => services
                .Configure<ApplicationSettings>(
                    configuration.GetSection(nameof(ApplicationSettings)),
                    options => options.BindNonPublicProperties = true)
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
                .AddHostedService<QueuedHostedService>()
                .AddSingleton<IBackgroundTaskQueue>(ctx =>
                {
                    if (!int.TryParse(configuration["QueueCapacity"], out var queueCapacity))
                    {
                        queueCapacity = 100;
                    }
                    return new BackgroundTaskQueue(queueCapacity);
                });
    }
}
