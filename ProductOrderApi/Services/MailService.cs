using MailKit.Net.Smtp;
using MimeKit;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Services
{
    internal class MailService : IMailService
    {
        private readonly string sender = "OrderProductApi Support";

        private readonly string smtpHost;
        private readonly string smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPassword;

        public MailService(IConfiguration configuration)
        {
            smtpHost = configuration["SMTP:Host"] ?? throw new InvalidOperationException("SMTP:Host is missing");
            smtpPort = configuration["SMTP:Port"] ?? throw new InvalidOperationException("SMTP:Host is missing");
            smtpUser = configuration["SMTP:User"] ?? throw new InvalidOperationException("SMTP:User is missing");
            smtpPassword = configuration["SMTP:Password"] ?? throw new InvalidOperationException("SMTP:Password is missing");
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(sender, smtpUser));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(smtpHost, int.Parse(smtpPort), MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(smtpUser, smtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
