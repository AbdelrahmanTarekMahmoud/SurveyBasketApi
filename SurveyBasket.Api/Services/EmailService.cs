

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.Api.Setting;
using System.Net.Mail;

namespace SurveyBasket.Api.Services
{
    public class EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger) : IEmailSender
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;
        private readonly ILogger<EmailService> _logger = logger;


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                //_mailSettings.Mail is the mail i will sent from 
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = subject
            };
            message.To.Add(MailboxAddress.Parse(email));

            var Body = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = Body.ToMessageBody();


            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            _logger.LogInformation("Sending email to {email}", email);
            _logger.LogInformation("Sending email using SMTP Host: {Host} and Port: {Port}{password}", _mailSettings.Host, _mailSettings.Port , _mailSettings.Password);


            smtp.Connect(_mailSettings.Host , _mailSettings.Port , SecureSocketOptions.None);
            //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);

            
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }
    }
}
