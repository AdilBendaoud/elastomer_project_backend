namespace projetStage.DTO.demandes
{
    public class CreateDemandeArticleModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string FamilleDeProduit { get; set; }
        public string Destination { get; set; }
    }
}
