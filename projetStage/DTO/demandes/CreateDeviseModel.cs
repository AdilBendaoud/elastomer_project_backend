namespace projetStage.DTO.demandes
{
    public class CreateDeviseModel
    {
        public int UserCode { get; set; }
        public string DemandeCode { get; set; }
        public List<DevisModel> DevisList { get; set; }
    }
}
