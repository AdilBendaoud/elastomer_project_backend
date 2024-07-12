namespace projetStage.DTO
{
    public class UserDetailsModel
    {

        public int Id { get; set; }
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Departement { get; set; }
        public bool IsActive { get; set; }

    }
}
