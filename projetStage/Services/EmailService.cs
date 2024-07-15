using System.Net.Mail;
using System.Net;
using projetStage.DTO;

namespace projetStage.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private SmtpClient _smtpClient;
        private string _from;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            UpdateSmtpClient();
        }

        public void SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            _smtpClient.Send(mailMessage);
        }

        public void SendEmailWithCC(string to, string subject, string body, string ccEmail)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            if (!string.IsNullOrEmpty(ccEmail))
            {
                mailMessage.CC.Add(ccEmail);
            }
            _smtpClient.Send(mailMessage);
        }

        public void UpdateSettings(EmailServiceSettingsModel settings)
        {
            var emailSettingsSection = _configuration.GetSection("EmailSettings");
            emailSettingsSection["SmtpServer"] = settings.SmtpServer;
            emailSettingsSection["Port"] = settings.Port.ToString();
            emailSettingsSection["Username"] = settings.Username;
            emailSettingsSection["Password"] = settings.Password;
            emailSettingsSection["EnableSsl"] = settings.EnableSsl.ToString();
            emailSettingsSection["From"] = settings.From;
            emailSettingsSection["UseAuthentication"] = settings.UseAuthentication.ToString();

            UpdateSmtpClient();
        }

        private void UpdateSmtpClient()
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            _smtpClient = new SmtpClient(emailSettings["SmtpServer"])
            {
                Port = int.Parse(emailSettings["Port"]),
                EnableSsl = bool.Parse(emailSettings["EnableSsl"])
            };

            if (bool.Parse(emailSettings["UseAuthentication"]))
            {
                _smtpClient.Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]);
            }

            _from = emailSettings["From"];
        }

    }
}
