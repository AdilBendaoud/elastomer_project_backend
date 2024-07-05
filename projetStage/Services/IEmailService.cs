using projetStage.DTO;

namespace projetStage.Services
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
        void UpdateSettings(EmailServiceSettingsModel settings);
    }
}
