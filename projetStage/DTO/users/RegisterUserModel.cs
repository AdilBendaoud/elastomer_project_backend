﻿namespace projetStage.DTO.users
{
    public class RegisterUserModel
    {
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Departement { get; set; }
        public bool ReOpenAfterValidation { get; set; }
    }
}
