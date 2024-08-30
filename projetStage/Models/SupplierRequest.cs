using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_supplierRequests")]
    public class SupplierRequest
    {
        public int Id { get; set; }
        public int DemandeId { get; set; }
        public Demande Demande { get; set; }
        public int SupplierId { get; set; }
        public Fournisseur Supplier { get; set; }
        public bool isSelectedForValidation { get; set; }
        public DateTime SentAt { get; set; }
    }
}
