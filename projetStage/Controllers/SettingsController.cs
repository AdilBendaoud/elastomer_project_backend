using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using projetStage.DTO;
using projetStage.Services;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public SettingsController(IConfiguration configuration, IEmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPut("update-email-settings")]
        [Authorize(Roles = "A")]
        public IActionResult UpdateEmailSettings([FromBody] EmailServiceSettingsModel model)
        {
            var emailSettingsSection = _configuration.GetSection("EmailSettings");
            emailSettingsSection["SmtpServer"] = model.SmtpServer;
            emailSettingsSection["Port"] = model.Port.ToString();
            emailSettingsSection["Username"] = model.Username;
            emailSettingsSection["Password"] = model.Password;
            emailSettingsSection["EnableSsl"] = model.EnableSsl.ToString();
            emailSettingsSection["From"] = model.From;
            emailSettingsSection["UseAuthentication"] = model.UseAuthentication.ToString();

            _emailService.UpdateSettings(model);

            return Ok("Email settings updated successfully.");
        }

        [HttpGet("get-email-settings")]
        [Authorize(Roles = "A")]
        public IActionResult GetEmailSettings()
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailServiceSettingsModel>();
            return Ok(emailSettings);
        }
    }
}
