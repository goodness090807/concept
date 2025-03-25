using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Shared.Interfaces;
using Shared.Settings;

namespace Shared
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceSetting _setting;
        public EmailService(IOptions<EmailServiceSetting> options)
        {
            _setting = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_setting.Name, _setting.UserName));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_setting.Host, _setting.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_setting.UserName, _setting.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
