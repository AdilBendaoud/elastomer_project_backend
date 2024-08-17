using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO.password;
using projetStage.Models;
using projetStage.Services;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IPasswordService _passwordService;

        public PasswordController(AppDbContext context, IPasswordService passwordService, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _passwordService = passwordService;
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return BadRequest("No user with that email address exists.");
            }
            var token = Guid.NewGuid().ToString();
            var resetToken = new PasswordResetToken
            {
                Email = model.Email,
                Token = BCrypt.Net.BCrypt.HashPassword(token),
                Expiration = DateTime.Now.AddHours(1) // Token valid for 1 hour
            };

            _context.PasswordResetTokens.Add(resetToken);
            _context.SaveChanges();

            await _emailService.SendEmail(model.Email, "Password Reset Request", $"<h3>Your reset code is: {token}</h3> <br> <p>please note this code is valid for 1 hour</p>");

            return Ok("Password reset code has been sent to your email.");
        }

        [HttpPost("validate-reset-code")]
        public IActionResult ValidateResetCode([FromBody] PasswordResetValidationModel model)
        {
            var resetToken = _context.PasswordResetTokens.OrderByDescending(t=> t.Expiration).FirstOrDefault(t => t.Email == model.Email);

            if (resetToken == null || !BCrypt.Net.BCrypt.Verify(model.ResetCode, resetToken.Token) || resetToken.Expiration < DateTime.Now)
            {
                return BadRequest("Invalid or expired reset code.");
            }

            return Ok("Reset code is valid.");
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            var resetToken = _context.PasswordResetTokens.OrderByDescending(t => t.Expiration).FirstOrDefault(t => t.Email == model.Email);

            if (resetToken == null || !BCrypt.Net.BCrypt.Verify(model.ResetCode, resetToken.Token) || resetToken.Expiration < DateTime.Now)
            {
                return BadRequest("Invalid or expired reset code.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                return BadRequest("Invalid email.");
            }

            user.Password = _passwordService.HashPassword(model.NewPassword);
            _context.PasswordResetTokens.Remove(resetToken); // Remove the token after successful password reset
            _context.SaveChanges();

            return Ok("Password has been reset successfully.");
        }

        [HttpPost("change-password")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePasswordModel model)
        {
            var code = User.Claims.FirstOrDefault(c => c.Type == "code")?.Value;
            Console.WriteLine("this is the code {0}",code);
            if (code == null)
            {
                return Unauthorized();
            }

            var user = _context.Users.SingleOrDefault(u => u.Code == Int32.Parse(code));
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            user.Password = _passwordService.HashPassword(model.NewPassword);
            user.NeedsPasswordChange = false;
            _context.SaveChanges();

            return Ok("Password changed successfully.");
        }
    }
}
