using projetStage.DTO;

namespace projetStage.Services
{
    public interface IEmailService
    {
        public Task SendEmail(string to, string subject, string body);
        public Task SendEmail(string from, string to, string subject, string body);
        public Task SendEmailAsync(string subject, string body, List<string> toEmails, string ccEmail);
        void UpdateSettings(EmailServiceSettingsModel settings);
    }
}
