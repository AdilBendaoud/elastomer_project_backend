namespace projetStage.DTO.demandes
{
    public class DevisItemModel
    {
        public int Id { get; set; }
        public int DemandeArticleId { get; set; }
        public string Devise { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int Delay { get; set; }
    }
}
