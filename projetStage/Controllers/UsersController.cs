using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.DTO.password;
using projetStage.Helper;
using projetStage.Models;
using projetStage.Services;
using System.Linq;
using System.Threading.Tasks;

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

        private User GetUser(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Code == id);
            return user;
        }

        [HttpGet()]
        [Authorize(Roles = "A")]
        public IActionResult GetUsers([FromQuery] string search = null, [FromQuery] string role = null, [FromQuery] string department = null, int pageNumber = 1, int pageSize = 10)
        {
            IQueryable<UserDetailsModel> usersQuery = _context.Users
                .Select(u => new UserDetailsModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Code = u.Code,
                    Departement = u.Departement,
                    IsActive = u.IsActive,
                    IsAdmin = u.IsAdmin,
                    IsPurchaser = u.IsPurchaser,
                    IsRequester = u.IsRequester,
                    IsValidator = u.IsValidator
                });

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                string searchLower = search.ToLower();
                usersQuery = usersQuery.Where(u =>
                    u.Code.ToString().ToLower().Contains(searchLower) ||
                    u.FirstName.ToLower().Contains(searchLower) ||
                    u.LastName.ToLower().Contains(searchLower) ||
                    u.Email.ToLower().Contains(searchLower));
            }

            // Apply role filter if provided
            if (!string.IsNullOrEmpty(role))
            {
                switch (role)
                {
                    case "A":
                        usersQuery = usersQuery.Where(u => u.IsAdmin);
                        break;
                    case "P":
                        usersQuery = usersQuery.Where(u => u.IsPurchaser);
                        break;
                    case "D":
                        usersQuery = usersQuery.Where(u => u.IsRequester);
                        break;
                    case "V":
                        usersQuery = usersQuery.Where(u => u.IsValidator);
                        break;
                }
            }

            // Apply department filter if provided
            if (!string.IsNullOrEmpty(department))
            {
                usersQuery = usersQuery.Where(u => u.Departement == department);
            }

            var totalUsers = usersQuery.Count();
            var paginatedUsers = usersQuery
                .OrderBy(u => u.FirstName) // Add sorting if needed
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
            var user = GetUser(model.Code);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the new code is unique
            if (_context.Users.Any(u => u.Code == model.Code && u.Id != model.Id))
            {
                return BadRequest("Code must be unique across all users.");
            }

            UpdateUserDetails(user, model);
            _context.SaveChanges();

            return Ok("User updated successfully.");
        }

        private void UpdateUserDetails(User user, UpdateUserModel model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Code = model.Code;
            user.Departement = model.Departement;
            user.IsAdmin = model.IsAdmin;
            user.IsPurchaser = model.IsPurchaser;
            user.IsRequester = model.IsRequester;
            user.IsValidator = model.IsValidator;
        }

        [HttpPatch("{userId}/block")]
        [Authorize(Roles = "A")]
        public IActionResult BlockUser(string userId)
        {
            var user = GetUser(Int32.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = false;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{userId}/unblock")]
        [Authorize(Roles = "A")]
        public async Task<IActionResult> UnBlockUser(string userId)
        {
            var user = GetUser(Int32.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = true;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{userId}/change-password")]
        [Authorize(Roles = "A")]
        public IActionResult ChangeUserPassword(string userId, [FromBody] ChangePasswordModel model)
        {
            User user = GetUser(Int32.Parse(userId));
            if (user == null)
            {
                return NotFound("user not found !!");
            }

            user.Password = _passwordService.HashPassword(model.NewPassword);
            _context.SaveChanges();
            return Ok("Password changed successfully!");
        }
    }
}
