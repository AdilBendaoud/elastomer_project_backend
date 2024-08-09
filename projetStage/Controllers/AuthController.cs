using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.DTO.users;
using projetStage.Helper;
using projetStage.Models;
using projetStage.Services;
using System.Linq;

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

        private bool CodeExists(int code)
        {
            return _context.Users.Any(u => u.Code == code);
        }

        private async void CreateUser(RegisterUserModel model, bool isAdmin, bool isRequester, bool isPurchaser, bool isValidator, bool reOpenAfterValidation)
        {
            if (CodeExists(model.Code))
            {
                throw new Exception("User with this code already exists.");
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                throw new Exception("User with this email already exists.");
            }

            var password = PasswordGenerator.GeneratePassword();
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = _passwordService.HashPassword(password),
                Code = model.Code,
                Departement = model.Departement,
                NeedsPasswordChange = true,
                IsActive = true,
                IsAdmin = isAdmin,
                IsPurchaser = isPurchaser,
                IsRequester = isRequester,
                IsValidator = isValidator,
                ReOpenRequestAfterValidation = reOpenAfterValidation
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            await _emailService.SendEmail(model.Email, "Account Created", $"Your temporary password is: {password}. Please log in and change your password.");
        }

        [HttpPost("register/admin")]
        
        public IActionResult RegisterAdmin([FromBody] RegisterUserModel model)
        {
            try
            {
                CreateUser(model, true, true, false, false, false);
                return Ok("Admin registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/acheteur")]
        //[Authorize(Roles = "A")]
        public IActionResult RegisterAcheteur([FromBody] RegisterUserModel model)
        {
            try
            {
                CreateUser(model, false, true, true, false, model.ReOpenAfterValidation);
                return Ok("Purchaser registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/demandeur")]
        //[Authorize(Roles = "A")]
        public IActionResult RegisterDemandeur([FromBody] RegisterUserModel model)
        {
            try
            {
                CreateUser(model, false, true, false, false, false);
                return Ok("Requester registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/validateur")]
        //[Authorize(Roles = "A")]
        public IActionResult RegisterValidateur([FromBody] RegisterUserModel model)
        {
            try
            {
                CreateUser(model, false, false, false, true, false);
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
            var user = _context.Users.SingleOrDefault(u => u.Code == model.Code);
            if (user != null && _passwordService.VerifyPassword(user.Password, model.Password))
            {
                var roles = new List<string>();
                if (user.IsAdmin) roles.Add("A");
                if (user.IsPurchaser) roles.Add("P");
                if (user.IsRequester) roles.Add("D");
                if (user.IsValidator) roles.Add("V");

                var token = _tokenService.GenerateToken(user.Code.ToString(), roles);

                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Code,
                        user.Departement,
                        user.NeedsPasswordChange,
                        user.IsActive,
                        Roles = roles,
                        user.ReOpenRequestAfterValidation
                    }
                });
            }

            return Unauthorized();
        }
    }
}
