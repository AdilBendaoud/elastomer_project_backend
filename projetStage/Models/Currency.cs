using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_currencies")]
    public class Currency
    {
        public int Id { get; set; }
        public String CurrencyCode { get; set; }
        public float PriceInEur { get; set; }
    }
}
