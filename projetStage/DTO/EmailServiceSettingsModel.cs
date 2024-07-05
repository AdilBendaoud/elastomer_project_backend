namespace projetStage.DTO
{
    public class EmailServiceSettingsModel
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string From { get; set; }
        public bool UseAuthentication { get; set; }
    }
}
