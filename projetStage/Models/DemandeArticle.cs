﻿using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_demandeArticles")]
    public class DemandeArticle
    {
        public int Id { get; set; }
        public int DemandeId { get; set; }
        public Demande Demande { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Qtt { get; set; }
        public string? BonCommande { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FamilleDeProduit { get; set; }
        public string? Destination { get; set; }
        public ICollection<DevisItem> DevisItems { get; set; }
    }
}