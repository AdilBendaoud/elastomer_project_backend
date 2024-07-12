namespace projetStage.Models
{
    public class Validateur : User
    {
        public ICollection<Demande> DemandesCFO { get; set; }
        public ICollection<Demande> DemandesCOO { get; set; }
    }
}