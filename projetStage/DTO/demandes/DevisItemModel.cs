namespace projetStage.DTO.demandes
{
    public class DevisItemModel
    {
        public int Id { get; set; }
        public int DemandeArticleId { get; set; }
        public string Devise { get; set; }
        public decimal UnitPrice { get; set; }
        public string Delay { get; set; }
    }
}
