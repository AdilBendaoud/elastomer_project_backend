﻿namespace projetStage.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<DemandeArticle> DemandeArticles { get; set; }
    }
}