namespace projetStage.DTO
{
    public class UserDetailsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Code { get; set; }
        public string Departement { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPurchaser { get; set; }
        public bool IsRequester { get; set; }
        public bool IsValidator { get; set; }
    }
}
