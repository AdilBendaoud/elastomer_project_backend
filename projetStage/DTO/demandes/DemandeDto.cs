using projetStage.Models;

namespace projetStage.DTO.demandes
{
    public class DemandeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DemandeStatus Status { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? LastModification { get; set; }
        public DemandeurDto Demandeur { get; set; }
    }
}
