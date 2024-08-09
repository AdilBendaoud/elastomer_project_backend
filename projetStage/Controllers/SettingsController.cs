using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using projetStage.Data;
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
        private readonly AppDbContext _context;

        public SettingsController(IConfiguration configuration, IEmailService emailService, AppDbContext context)
        {
            _emailService = emailService;
            _configuration = configuration;
            _context = context;
        }

        [HttpPut("update-email-settings")]
        //[Authorize(Roles = "A")]
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
        //[Authorize(Roles = "A")]
        public IActionResult GetEmailSettings()
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailServiceSettingsModel>();
            return Ok(emailSettings);
        }

        [HttpGet("get-currency-settings")]
        public IActionResult GetCurrencies()
        {         
            var currencies = _context.Currencies;
            CurrencyModel model = new();
            foreach (var currency in currencies) {
                if(currency.CurrencyCode == "USD"){
                    model.UsdToEur = currency.PriceInEur;
                }else if(currency.CurrencyCode == "MAD"){
                    model.MadToEur = currency.PriceInEur;
                }
                else if(currency.CurrencyCode == "GBP"){
                    model.GbpToEur = currency.PriceInEur;
                }
            }
            return Ok(model);
        }

        [HttpPut("update-currency-settings")]
        public IActionResult UpdateCurrency([FromBody] CurrencyModel model)
        {
            var mad = _context.Currencies.First(c => c.CurrencyCode == "MAD");
            mad.PriceInEur = model.MadToEur;
            var usd = _context.Currencies.First(c => c.CurrencyCode == "USD");
            usd.PriceInEur = model.UsdToEur;
            var gbp = _context.Currencies.First(c => c.CurrencyCode == "GBP");
            gbp.PriceInEur = model.GbpToEur;
            _context.SaveChanges();
            return Ok();
        }
    }
}
