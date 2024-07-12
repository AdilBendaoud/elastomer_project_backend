namespace projetStage.Models
{
    public class Devis
    {
        public int Id { get; set; }
        public decimal Prix { get; set; }
        public int Qtt { get; set; }
        public DateTime DateReception { get; set; }
        public string Devise { get; set; }
        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; }
        public int DemandeId { get; set; }
        public Demande Demande { get; set; }
    }
}
