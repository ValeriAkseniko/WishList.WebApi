using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;

namespace WishList.BusinessLogicServices.EmailService
{
    public class EmailClient : IEmailClient
    {
        private readonly SmtpClient smtpClient;
        private readonly ILogger<EmailClient> logger;
        private readonly IConfiguration configuration;
        public EmailClient(ILogger<EmailClient> logger, IConfiguration configuration)
        {
            smtpClient = new SmtpClient()
            {
                EnableSsl = bool.Parse(configuration["EmailOptions:EnableSsl"]),
                Port = int.Parse(configuration["EmailOptions:Port"]),
                Host = configuration["EmailOptions:Host"],
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(configuration["EmailOptions:User"], configuration["EmailOptions:Password"]),
            };
            this.logger = logger;
            this.configuration = configuration;
        }
        public async Task SendAsync(string emailTo, string message, string subject)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(emailTo);
                mailMessage.From = new MailAddress(configuration["EmailOptions:From"]);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                await smtpClient.SendMailAsync(mailMessage);
                logger.LogInformation($"send email message to {emailTo}");
            }
            catch (Exception ex)
            {
                logger.LogError($"error when sending email message:{ex.Message}");
            }
        }
    }
}
