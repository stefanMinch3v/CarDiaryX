using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Common.BackgroundServices
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, Task<IRequest<Result>>> workItem);
        ValueTask<Func<CancellationToken, Task<IRequest<Result>>>> DequeueAsync(CancellationToken cancellationToken);
        int WorkItemsCount { get; }
    }
}
