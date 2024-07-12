namespace projetStage.DTO.demandes
{
    public class UpdateArticlesModel
    {
        public int userCode {  get; set; }
        public string DemandeCode { get; set; }
        public List<CreateDemandeArticleModel> Articles { get; set; }
    }
}
