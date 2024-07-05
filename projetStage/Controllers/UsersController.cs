using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.Helper;
using projetStage.Migrations;
using projetStage.Models;
using projetStage.Services;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public UsersController(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [HttpGet("users")]
        [Authorize(Roles = "A")]
        public IActionResult GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {
            var users = _context.Admins
                .Select(u => new UserDetailsModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                })
                .Union(_context.Acheteurs
                    .Select(u => new UserDetailsModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Role = u.Role,
                        Code = u.Code,
                        Departement = u.Departement
                    }))
                .Union(_context.Demandeurs
                    .Select(u => new UserDetailsModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Role = u.Role,
                        Code = u.Code,
                        Departement = u.Departement
                    }))
                .Union(_context.Validateurs
                    .Select(u => new UserDetailsModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Role = u.Role,
                        Code = u.Code,
                        Departement = u.Departement
                    }))
                .ToList();

            var totalUsers = users.Count;
            var paginatedUsers = users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new PaginatedResponse<UserDetailsModel>
            {
                TotalCount = totalUsers,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Items = paginatedUsers
            };

            return Ok(response);
        }

        [HttpGet("users/filter")]
        [Authorize(Roles = "A")]
        public IActionResult GetFilteredUsers([FromQuery] string role = null, [FromQuery] string departement = null, int pageNumber = 1, int pageSize = 10)
        {
            var users = _context.Admins.AsQueryable()
                .Select(u => new UserDetailsModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Code = u.Code,
                    Departement = u.Departement
                })
                .Union(_context.Acheteurs.AsQueryable()
                    .Select(u => new UserDetailsModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Role = u.Role,
                        Code = u.Code,
                        Departement = u.Departement
                    }))
                .Union(_context.Demandeurs.AsQueryable()
                    .Select(u => new UserDetailsModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Role = u.Role,
                        Code = u.Code,
                        Departement = u.Departement
                    }))
                .Union(_context.Validateurs.AsQueryable()
                    .Select(u => new UserDetailsModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Role = u.Role,
                        Code = u.Code,
                        Departement = u.Departement
                    }));

            if (!string.IsNullOrEmpty(role))
            {
                users = users.Where(u => u.Role == role);
            }

            if (!string.IsNullOrEmpty(departement))
            {
                users = users.Where(u => u.Departement == departement);
            }

            var totalUsers = users.Count();
            var paginatedUsers = users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new PaginatedResponse<UserDetailsModel>
            {
                TotalCount = totalUsers,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Items = paginatedUsers
            };

            return Ok(response);
        }

        [HttpPut("update-user")]
        [Authorize(Roles = "A")]
        public IActionResult UpdateUser([FromBody] UpdateUserModel model)
        {
            var user = _context.Admins.SingleOrDefault(u => u.Code == model.Code) ??
                       (User)_context.Acheteurs.SingleOrDefault(u => u.Code == model.Code) ??
                       (User)_context.Demandeurs.SingleOrDefault(u => u.Code == model.Code) ??
                       (User)_context.Validateurs.SingleOrDefault(u => u.Code == model.Code);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the new code is unique across all tables
            if (_context.Admins.Any(u => u.Code == model.Code && u.Id != model.Id) ||
                _context.Acheteurs.Any(u => u.Code == model.Code && u.Id != model.Id) ||
                _context.Demandeurs.Any(u => u.Code == model.Code && u.Id != model.Id) ||
                _context.Validateurs.Any(u => u.Code == model.Code && u.Id != model.Id))
            {
                return BadRequest("Code must be unique across all users.");
            }

            string currentRole = user.GetType().GetProperty("Role").GetValue(user).ToString();
            if (currentRole != model.Role && !string.IsNullOrEmpty(model.Role))
            {
                // Remove user from current role table
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _context.SaveChanges();

                // Create user in the new role table
                CreateUserInNewRole(user, model.Role);
            }
            else
            {
                // Update user details without changing role
                UpdateUserDetails(user, model);
                _context.SaveChanges();
            }

            return Ok("User updated successfully.");
        }

        private void CreateUserInNewRole(User model, string newRole)
        {
            switch (newRole)
            {
                case "A":
                    var admin = new Admin
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        Role = newRole,
                        Code = model.Code,
                        Departement = model.Departement,
                        NeedsPasswordChange = true
                    };
                    _context.Admins.Add(admin);
                    _context.SaveChanges();
                    break;

                case "P":
                    var acheteur = new Acheteur
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        Role = newRole,
                        Code = model.Code,
                        Departement = model.Departement,
                        NeedsPasswordChange = true
                    };
                    _context.Acheteurs.Add(acheteur);
                    _context.SaveChanges();
                    break;

                case "D":
                    var demandeur = new Demandeur
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        Role = newRole,
                        Code = model.Code,
                        Departement = model.Departement,
                        NeedsPasswordChange = true
                    };
                    _context.Demandeurs.Add(demandeur);
                    _context.SaveChanges();
                    break;

                case "V":
                    var validateur = new Validateur
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        Role = newRole,
                        Code = model.Code,
                        Departement = model.Departement,
                        NeedsPasswordChange = true
                    };
                    _context.Validateurs.Add(validateur);
                    _context.SaveChanges();
                    break;
                default:
                    throw new Exception("Invalid role specified.");
            }
        }

        private void UpdateUserDetails(object user, UpdateUserModel model)
        {
            user.GetType().GetProperty("FirstName").SetValue(user, model.FirstName);
            user.GetType().GetProperty("LastName").SetValue(user, model.LastName);
            user.GetType().GetProperty("Email").SetValue(user, model.Email);
            user.GetType().GetProperty("Code").SetValue(user, model.Code);
            user.GetType().GetProperty("Departement").SetValue(user, model.Departement);
        }
    }
}