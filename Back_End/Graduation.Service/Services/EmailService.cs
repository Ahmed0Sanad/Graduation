using Graduation.Data.Commons;
using Graduation.Data.Helper;
using Graduation.Service.Abstract;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Graduation.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<IEmailService> _logger;

        public EmailService(IOptions<EmailSettings> options , ILogger<IEmailService> logger)
        {
            this._emailSettings = options.Value;
            this._logger = logger;
        }
        public async Task<bool> SendEmailAsync(Email email)
        {
            try
            {

                // create message
                var message = new MimeMessage();
                message.Sender = new MailboxAddress(_emailSettings.DisplayName, _emailSettings.EmailFrom);
                message.To.Add(MailboxAddress.Parse(email.To));
                message.Subject = email.Subject;

                var builder = new BodyBuilder();
                if ( email.IsBodyHtml)
                {
                    builder.HtmlBody = email.Body; // Use HTML body
                }
                else
                {
                    builder.TextBody = email.Body; // Use plain text body
                }

                message.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettings.SmtpHost, Convert.ToInt32(_emailSettings.SmtpPort), SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                await smtp.SendAsync(message);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                // Handle exception (you might want to log it)
                _logger.LogError(ex, "Error sending email");
                return false;
            }

            return true;
        }
    }
}
