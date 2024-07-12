namespace projetStage.Models
{
    public class Fournisseur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public ICollection<Devis> Devis { get; set; }
    }
}
