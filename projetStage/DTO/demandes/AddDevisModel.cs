namespace projetStage.DTO.demandes
{
    public class AddDevisModel
    {
        public int SupplierId { get; set; }
        public List<DevisItemModel> Items { get; set; }
    }
}
