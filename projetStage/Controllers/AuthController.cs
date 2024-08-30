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
using System.Text;

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
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, IPasswordService passwordService, IEmailService emailService, AppDbContext context, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _passwordService = passwordService;
            _emailService = emailService;
            _context = context;
            _configuration = configuration;
        }

        private bool CodeExists(int code)
        {
            return _context.Users.Any(u => u.Code == code);
        }

        private async void CreateUser(RegisterUserModel model, bool isAdmin, bool isRequester, bool isPurchaser, bool isValidator, bool reOpenAfterValidation)
        {
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

            string url = _configuration["Variables:URL"];
            await _emailService.SendEmail(model.Email, "Account Created", $"<h3>Your temporary password is: {password} </h3> <br> <p>Please head to <a href=\"{url}\" target=\"_blank\">the website</a>, log in and change your password.</p>");
        }

        [HttpPost("register/admin")]
        
        public IActionResult RegisterAdmin([FromBody] RegisterUserModel model)
        {
            var existes = CodeExists(model.Code);
            if (existes)
            {
                return BadRequest("User with this code already exists.");
            }

            var emailExistes = _context.Users.Any(u => u.Email == model.Email);

            if (emailExistes)
            {
                return BadRequest("User with this email already exists.");
            }

            CreateUser(model, true, true, false, false, false);
            return Ok("Admin registered successfully.");
        }

        [HttpPost("register/acheteur")]
        //[Authorize(Roles = "A")]
        public IActionResult RegisterAcheteur([FromBody] RegisterUserModel model)
        {
            var existes = CodeExists(model.Code);
            if (existes)
            {
                return BadRequest("User with this code already exists.");
            }

            var emailExistes = _context.Users.Any(u => u.Email == model.Email);

            if (emailExistes)
            {
                return BadRequest("User with this email already exists.");
            }

            CreateUser(model, false, true, true, false, model.ReOpenAfterValidation);
            return Ok("Purchaser registered successfully.");
        }

        [HttpPost("register/demandeur")]
        //[Authorize(Roles = "A")]
        public IActionResult RegisterDemandeur([FromBody] RegisterUserModel model)
        {
            var existes = CodeExists(model.Code);
            if (existes)
            {
                return BadRequest("User with this code already exists.");
            }

            var emailExistes = _context.Users.Any(u => u.Email == model.Email);

            if (emailExistes)
            {
                return BadRequest("User with this email already exists.");
            }

            CreateUser(model, false, true, false, false, false);
            return Ok("Requester registered successfully.");
        }

        [HttpPost("register/validateur")]
        //[Authorize(Roles = "A")]
        public IActionResult RegisterValidateur([FromBody] RegisterUserModel model)
        {
            var existes = CodeExists(model.Code);
            if (existes)
            {
                return BadRequest("User with this code already exists.");
            }

            var emailExistes = _context.Users.Any(u => u.Email == model.Email);

            if (emailExistes)
            {
                return BadRequest("User with this email already exists.");
            }

            CreateUser(model, false, false, false, true, false);
            return Ok("Validator registered successfully.");
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
