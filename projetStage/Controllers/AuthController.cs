using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projetStage.Data;
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

        public AuthController(ITokenService tokenService, IPasswordService passwordService, AppDbContext context)
        {
            _tokenService = tokenService;
            _passwordService = passwordService;
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

        [HttpPost("register/admin")]
        public IActionResult RegisterAdmin([FromBody] Admin admin)
        {
            if (CodeExists(admin.Code))
            {
                return BadRequest("User with this code already exists.");
            }
            admin.Password = _passwordService.HashPassword(admin.Password);
            _context.Admins.Add(admin);
            _context.SaveChanges();

            return Ok("Admin registered successfully.");
        }

        [HttpPost("register/acheteur")]
        public IActionResult RegisterAcheteur([FromBody] Acheteur acheteur)
        {
            if (CodeExists(acheteur.Code))
            {
                return BadRequest("User with this code already exists.");
            }

            acheteur.Password = _passwordService.HashPassword(acheteur.Password);
            _context.Acheteurs.Add(acheteur);
            _context.SaveChanges();

            return Ok("Acheteur registered successfully.");
        }

        [HttpPost("register/demandeur")]
        public IActionResult RegisterDemandeur([FromBody] Demandeur demandeur)
        {
            if (CodeExists(demandeur.Code))
            {
                return BadRequest("User with this code already exists.");
            }

            demandeur.Password = _passwordService.HashPassword(demandeur.Password);
            _context.Demandeurs.Add(demandeur);
            _context.SaveChanges();

            return Ok("Demandeur registered successfully.");
        }

        [HttpPost("register/validateur")]
        public IActionResult RegisterValidateur([FromBody] Validateur validateur)
        {
            if (CodeExists(validateur.Code))
            {
                return BadRequest("User with this code already exists.");
            }

            validateur.Password = _passwordService.HashPassword(validateur.Password);
            _context.Validateurs.Add(validateur);
            _context.SaveChanges();

            return Ok("Validateur registered successfully.");
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
