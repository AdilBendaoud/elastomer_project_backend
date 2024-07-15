using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_demandeHistories")]
    public class DemandeHistory
    {
        public int Id { get; set; }
        public int DemandeId { get; set; }
        public Demande Demande { get; set; }
        public int UserCode { get; set; }
        public DateTime DateChanged { get; set; }
        public string Details { get; set; }
    }
}
