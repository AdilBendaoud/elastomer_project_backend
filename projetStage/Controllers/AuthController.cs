using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.Helper;
using projetStage.Models;
using projetStage.Services;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AuthController(ITokenService tokenService, IPasswordService passwordService, IEmailService emailService, AppDbContext context)
        {
            _tokenService = tokenService;
            _passwordService = passwordService;
            _emailService = emailService;
            _context = context;
        }

        private bool CodeExists (int code)
        {
            if (_context.Admins.Any(u => u.Code == code) ||
                _context.Acheteurs.Any(u => u.Code == code) ||
                _context.Validateurs.Any(u => u.Code == code) || 
                _context.Demandeurs.Any(u => u.Code == code)) 
            {
                return true;
            }
            return false;
        }

        private void CreateUser<T>(RegisterUserModel model, string role, DbSet<T> dbSet) where T : class
        {
            if (CodeExists(model.Code))
            {
                throw new Exception("User with this code already exists.");
            }

            if (_context.Set<T>().Any(u => EF.Property<string>(u, "Email") == model.Email))
            {
                throw new Exception("User with this email already exists.");
            }
            var password = PasswordGenerator.GeneratePassword();
            var user = (T)Activator.CreateInstance(typeof(T));
            user.GetType().GetProperty("FirstName").SetValue(user, model.FirstName);
            user.GetType().GetProperty("LastName").SetValue(user, model.LastName);
            user.GetType().GetProperty("Email").SetValue(user, model.Email);
            user.GetType().GetProperty("Password").SetValue(user, _passwordService.HashPassword(password));
            user.GetType().GetProperty("Role").SetValue(user, role);
            user.GetType().GetProperty("Code").SetValue(user, model.Code);
            user.GetType().GetProperty("Departement").SetValue(user, model.Departement);
            user.GetType().GetProperty("NeedsPasswordChange").SetValue(user, true);

            dbSet.Add(user);
            _context.SaveChanges();

            _emailService.SendEmail(model.Email, "Account Created", $"Your temporary password is: {password}. Please log in and change your password.");
        }

        [HttpPost("register/admin")]
        [Authorize(Roles = "A")]
        public IActionResult RegisterAdmin([FromBody] RegisterUserModel admin)
        {
            try
            {
                CreateUser(admin, "A", _context.Admins);
                return Ok("Admin registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/acheteur")]
        [Authorize(Roles = "A")]
        public IActionResult RegisterAcheteur([FromBody] RegisterUserModel acheteur)
        {
            try
            {
                CreateUser(acheteur, "P", _context.Acheteurs);
                return Ok("Purchaseur registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/demandeur")]
        [Authorize(Roles = "A")]
        public IActionResult RegisterDemandeur([FromBody] RegisterUserModel demandeur)
        {
            try
            {
                CreateUser(demandeur, "D", _context.Demandeurs);
                return Ok("Requesteur registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/validateur")]
        [Authorize(Roles = "A")]
        public IActionResult RegisterValidateur([FromBody] RegisterUserModel validateur)
        {
            try
            {
                CreateUser(validateur, "V", _context.Validateurs);
                return Ok("Validator registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var admin = _context.Admins.SingleOrDefault(u => u.Code == model.Code);
            if (admin != null && _passwordService.VerifyPassword(admin.Password, model.Password))
            {
                var token = _tokenService.GenerateToken(admin.Code.ToString(), "A");
                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        admin.Id,
                        admin.FirstName,
                        admin.LastName,
                        admin.Email,
                        admin.Code,
                        admin.Departement,
                        admin.Role
                    }
                });
            }

            var acheteur = _context.Acheteurs.SingleOrDefault(u => u.Code == model.Code);
            if (acheteur != null && _passwordService.VerifyPassword(acheteur.Password, model.Password))
            {
                var token = _tokenService.GenerateToken(acheteur.Code.ToString(), "P");
                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        acheteur.Id,
                        acheteur.FirstName,
                        acheteur.LastName,
                        acheteur.Email,
                        acheteur.Code,
                        acheteur.Departement,
                        acheteur.Role
                    }
                });
            }

            var demandeur = _context.Demandeurs.SingleOrDefault(u => u.Code == model.Code);
            if (demandeur != null && _passwordService.VerifyPassword(demandeur.Password, model.Password))
            {
                var token = _tokenService.GenerateToken(demandeur.Code.ToString(), "D");
                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        demandeur.Id,
                        demandeur.FirstName,
                        demandeur.LastName,
                        demandeur.Email,
                        demandeur.Code,
                        demandeur.Departement,
                        demandeur.Role
                    }
                });
            }

            var validateur = _context.Validateurs.SingleOrDefault(u => u.Code == model.Code);
            if (validateur != null && _passwordService.VerifyPassword(validateur.Password, model.Password))
            {
                var token = _tokenService.GenerateToken(validateur.Code.ToString(), "V");
                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        validateur.Id,
                        validateur.FirstName,
                        validateur.LastName,
                        validateur.Email,
                        validateur.Code,
                        validateur.Departement,
                        validateur.Role
                    }
                });
            }

            return Unauthorized();
        }
    }
}
