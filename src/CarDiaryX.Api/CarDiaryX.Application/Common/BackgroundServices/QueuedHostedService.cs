using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Common.BackgroundServices
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio
    /// </summary>
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger<QueuedHostedService> logger;
        private readonly IMediator mediator;

        public QueuedHostedService(
            IBackgroundTaskQueue taskQueue, 
            ILogger<QueuedHostedService> logger,
            IMediator mediator)
        {
            this.TaskQueue = taskQueue;
            this.logger = logger;
            this.mediator = mediator;
        }

        public IBackgroundTaskQueue TaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation($"Queued Hosted Service is running.{Environment.NewLine}");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await this.TaskQueue.DequeueWorkItem(stoppingToken);

                try
                {
                    this.logger.LogInformation("Executing background task.");
                    var requestCommand = await workItem(stoppingToken);
                    await this.mediator.Send(requestCommand);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
