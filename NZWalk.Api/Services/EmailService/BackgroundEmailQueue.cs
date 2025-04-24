using NZWalk.Api.Models.EmailConfiguration;
using System.Threading;
using System.Threading.Channels;

namespace NZWalk.Api.Services.EmailService
{
    public class BackgroundEmailQueue : IBackgroundEmailQueue
    {
        private readonly Channel<EmailWorkItem> _queue;
        public BackgroundEmailQueue(int capacity = 100)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<EmailWorkItem>(options);

        }
        public async ValueTask<EmailWorkItem> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(EmailWorkItem workItem)
        {
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));
            await _queue.Writer.WriteAsync(workItem);
        }
    }
}
