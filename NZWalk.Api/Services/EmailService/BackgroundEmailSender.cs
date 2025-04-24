using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace NZWalk.Api.Services.EmailService
{
    public class BackgroundEmailSender : BackgroundService  //a base class from Microsoft.Extensions.Hosting.//It’s used to create long-running background tasks in ASP.NET Core.


    {
        private readonly IEmailService emailService;
        private readonly IBackgroundEmailQueue emailQueue;
        private readonly ILogger<BackgroundEmailSender> logger;

        public BackgroundEmailSender( IEmailService emailService,
            IBackgroundEmailQueue emailQueue, ILogger<BackgroundEmailSender>logger)
        {
            this.emailService = emailService;
            this.emailQueue = emailQueue;
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               
                try
                {
                    var workItem = await emailQueue.DequeueAsync(stoppingToken);
                    await emailService.SendEmailAsync(
                        workItem.Email,
                        workItem.Subject,
                        workItem.Message,
                        workItem.IsHtml
                    );
                    logger.LogInformation("Email sent successfully to {Email}", workItem.Email);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred sending email");
                }
            }
        }
    }
}
