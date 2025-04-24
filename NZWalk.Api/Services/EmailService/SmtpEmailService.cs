using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using NZWalk.Api.Models.EmailConfiguration;
using MailKit.Security;

namespace NZWalk.Api.Services.EmailService
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public SmtpEmailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Server, _settings.Port, _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);


        }
    }
}
