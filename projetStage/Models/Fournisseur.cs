using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_fournisseurs")]
    public class Fournisseur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public ICollection<Devis> Devis { get; set; }
    }
}
