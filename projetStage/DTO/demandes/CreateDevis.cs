namespace projetStage.DTO.demandes
{
    public class CreateDevis
    {
        public int UserCode { get; set; }
        public int FournisseurId { get; set; }
        public int DemandeId { get; set; }
        public decimal Prix { get; set; }
        public DateTime DateReception { get; set; }
        public int Quantity { get; set; }
        public string Devise { get; set; }
    }
}
