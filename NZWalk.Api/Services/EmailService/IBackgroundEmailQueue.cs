using NZWalk.Api.Models.EmailConfiguration;

namespace NZWalk.Api.Services.EmailService
{
    public interface IBackgroundEmailQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(EmailWorkItem workItem);
        ValueTask<EmailWorkItem> DequeueAsync(CancellationToken cancellationToken);
    }
}
