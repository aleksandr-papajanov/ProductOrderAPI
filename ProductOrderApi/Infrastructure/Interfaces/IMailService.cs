using MimeKit;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface IMailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
