namespace projetStage.Models
{
    public class DemandeArticle
    {
        public int DemandeId { get; set; }
        public Demande Demande { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Qtt { get; set; }
        public string Status { get; set; }
        public string? BonCommande { get; set; }
    }
}
