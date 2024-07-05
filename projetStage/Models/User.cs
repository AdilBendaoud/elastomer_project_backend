﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace projetStage.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Departement { get; set; }
        public bool NeedsPasswordChange { get; set; }
    }
}
