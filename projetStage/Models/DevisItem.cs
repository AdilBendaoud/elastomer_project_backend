namespace projetStage.Models
{
    public class DevisItem
    {
        public int Id { get; set; }
        public int DemandeArticleId { get; set; }
        public DemandeArticle DemandeArticle { get; set; }
        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Devise { get; set; }
        public int Delay {  get; set; }
    }
}
