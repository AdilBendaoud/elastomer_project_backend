namespace projetStage.Models
{
    public class DevisItem
    {
        public int Id { get; set; }
        public int DemandeArticleId { get; set; }
        public DemandeArticle DemandeArticle { get; set; }
        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; }
        public decimal UnitPrice { get; set; }
        public float? Discount { get; set; }
        public string Devise { get; set; }
        public string? Delay {  get; set; }
    }
}
