using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.Services.EmailService;

namespace NZWalk.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly ILogger<EmailTestController> logger;

        public EmailTestController(IEmailService emailService,ILogger<EmailTestController>logger)
        {
            this.emailService = emailService;
            this.logger = logger;
        }
        [HttpPost("send-test-email")]
        public async Task<IActionResult> SendTestEmail([FromBody] TestEmailRequest request)
        {
            try
            {
                await emailService.SendEmailAsync(
                       request.Email,
                       "NZWalks Test Email",
                       $"<h1>Hello from NZWalks!</h1><p>This is a test email sent at {DateTime.Now}</p>",
                       true
                );
                logger.LogInformation("Test email sent successfully");

                return Ok(new { Message = "Test email sent successfully" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[EmailTest Controller] Error occurred during Email test.");
                throw new Exception($"Error occurred during Email test", ex);

            }
        }
    }

    public record TestEmailRequest(string Email);
}

