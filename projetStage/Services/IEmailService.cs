using projetStage.DTO;

namespace projetStage.Services
{
    public interface IEmailService
    {
        void SendEmailWithCC(string to, string subject, string body, string ccEmail);
        void SendEmail(string to, string subject, string body);
        void UpdateSettings(EmailServiceSettingsModel settings);
    }
}
