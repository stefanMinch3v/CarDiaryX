using MediatR;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Common.BackgroundServices
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio
    /// </summary>
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task<IRequest<Result>>>> queue;

        public BackgroundTaskQueue(int capacity)
        {
            // Capacity should be set based on the expected application load and
            // number of concurrent threads accessing the queue.            
            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
            // which completes only when space became available. This leads to backpressure,
            // in case too many publishers/calls start accumulating.
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            this.queue = Channel.CreateBounded<Func<CancellationToken, Task<IRequest<Result>>>>(options);
        }

        /// <summary>
        /// https://github.com/dotnet/runtime/pull/312/files
        /// </summary>
        public int WorkItemsCount
        {
            get
            {
                if (!this.queue.Reader.CanCount)
                {
                    return -1;
                }

                return this.queue.Reader.Count;
            }
        }

        public ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, Task<IRequest<Result>>> workItem)
        {
            if (workItem is null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            return this.queue.Writer.WriteAsync(workItem);
        }

        public ValueTask<Func<CancellationToken, Task<IRequest<Result>>>> DequeueAsync(CancellationToken cancellationToken)
            => this.queue.Reader.ReadAsync(cancellationToken);     
    }
}
