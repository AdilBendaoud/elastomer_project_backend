namespace projetStage.DTO
{
    public class UpdateUserModel
    {
        public int Id { get; set; } // User ID to identify which user to update
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Code { get; set; }
        public string Departement { get; set; }
        public string Role { get; set; } // Optional: Allow updating user role if needed
    }
}
