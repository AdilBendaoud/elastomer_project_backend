namespace projetStage.DTO.demandes
{
    public class CreateDemandeModel
    {
        public string Code { get; set; }
        public int DemandeurCode {  get; set; }
        public int DemandeurId { get; set; }
        public List<CreateDemandeArticleModel> Articles { get; set; }
    }
}
