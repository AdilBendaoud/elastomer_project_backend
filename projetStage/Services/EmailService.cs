using System.Net.Mail;
using System.Net;
using projetStage.DTO;
using System.Net.Http;

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

        public async Task SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendEmail(string from, string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendEmailAsync(string subject, string body, List<string> toEmails, string ccEmail)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var toEmail in toEmails)
            {
                mailMessage.To.Add(toEmail);
            }

            // Add CC email address
            if (!string.IsNullOrEmpty(ccEmail))
            {
                mailMessage.CC.Add(ccEmail);
            }

            await _smtpClient.SendMailAsync(mailMessage);
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
