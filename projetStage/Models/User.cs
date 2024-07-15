using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_users")]
    public class User
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Departement { get; set; }
        public bool NeedsPasswordChange { get; set; }
        public bool IsActive { get; set; }

        // Role attributes
        public bool IsAdmin { get; set; }
        public bool IsPurchaser { get; set; }
        public bool IsRequester { get; set; }
        public bool IsValidator { get; set; }

        // Navigation properties
        public ICollection<Demande> Demandes { get; set; }
    }

}
