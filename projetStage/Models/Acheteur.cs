namespace projetStage.Models
{
    public class Acheteur : User
    {
        public ICollection<Demande> Demandes { get; set; }
    }
}
