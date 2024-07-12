namespace projetStage.Models
{
    public class Demandeur : User
    {
        public ICollection<Demande> Demandes { get; set; }
    }
}
