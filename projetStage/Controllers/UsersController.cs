using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.Models;
using projetStage.Services;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("users")]
        [Authorize(Roles = "A")]
        public IActionResult GetAllUsers()
        {
            var users = new List<User>();

            users.AddRange(_context.Admins.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                Code = u.Code,
                Departement = u.Departement
            }));

            users.AddRange(_context.Acheteurs.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                Code = u.Code,
                Departement = u.Departement
            }));

            users.AddRange(_context.Demandeurs.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                Code = u.Code,
                Departement = u.Departement
            }));

            users.AddRange(_context.Validateurs.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                Code = u.Code,
                Departement = u.Departement
            }));

            return Ok(users);
        }

        [HttpGet("users/filter")]
        [Authorize(Roles = "A")]
        public IActionResult GetFilteredUsers([FromQuery] string role = null, [FromQuery] string departement = null)
        {
            var users = new List<User>();

            if (string.IsNullOrEmpty(role) && string.IsNullOrEmpty(departement))
            {
                return BadRequest("At least one filter parameter must be provided.");
            }

            if (!string.IsNullOrEmpty(role))
            {
                users.AddRange(_context.Admins.Where(u => u.Role == role).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));

                users.AddRange(_context.Acheteurs.Where(u => u.Role == role).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));

                users.AddRange(_context.Demandeurs.Where(u => u.Role == role).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));

                users.AddRange(_context.Validateurs.Where(u => u.Role == role).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));
            }

            if (!string.IsNullOrEmpty(departement))
            {
                users.AddRange(_context.Admins.Where(u => u.Departement == departement).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));

                users.AddRange(_context.Acheteurs.Where(u => u.Departement == departement).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));

                users.AddRange(_context.Demandeurs.Where(u => u.Departement == departement).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));

                users.AddRange(_context.Validateurs.Where(u => u.Departement == departement).Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                }));
            }

            return Ok(users);
        }
    }
}