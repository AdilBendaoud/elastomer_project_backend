using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_articles")]
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string FamilleDeProduit { get; set; }
        public string Destination { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}