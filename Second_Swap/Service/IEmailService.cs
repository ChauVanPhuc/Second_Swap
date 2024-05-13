using MailKit.Net.Smtp;
using MimeKit;

namespace Second_Swap.Service
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string html);
    }   
}
