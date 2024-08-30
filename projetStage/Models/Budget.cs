using System.ComponentModel.DataAnnotations.Schema;

namespace projetStage.Models
{
    [Table("WESM_Budgets")]
    public class Budget
    {
        public int Id { get; set; }
        public string Departement { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int? InitialBudget { get; set; }
        public int? SalesBudget { get; set; }
        public int? SalesForecast { get; set; }
        public int? Adjustment { get; set; }
        public int? BudgetIP { get; set; }
    }
}