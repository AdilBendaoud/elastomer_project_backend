namespace projetStage.Models
{
    public class Demande
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DemandeStatus Status { get; set; }

        public int DemandeurId { get; set; }
        public Demandeur Demandeur { get; set; }
        public DateTime? OpenedAt { get; set; }

        public int? ValidateurCFOId { get; set; }
        public Validateur ValidateurCFO { get; set; }
        public string? CommentCFO { get; set; }
        public bool IsValidateurCFOValidated { get; set; }
        public bool IsValidateurCFORejected { get; set; }
        public DateTime? ValidatedOrRejectedByCFOAt { get; set; }

        public int? ValidateurCOOId { get; set; }
        public Validateur ValidateurCOO { get; set; }
        public string? CommentCOO { get; set; }
        public bool IsValidateurCOOValidated { get; set; }
        public bool IsValidateurCOORejected { get; set; }
        public DateTime? ValidatedOrRejectedByCOOAt { get; set; }

        public int? AcheteurId { get; set; }
        public Acheteur Acheteur { get; set; }

        public ICollection<DemandeArticle> DemandeArticles { get; set; }
        public ICollection<DemandeHistory> DemandeHistories { get; set; }
        public ICollection<Devis> Devis { get; set; }
    }
}