namespace projetStage.DTO
{
    public class BudgetResponse
    {
        public List<int> InitialBudget { get; set; }
        public List<int> SalesBudget { get; set; }
        public List<int> SalesForecast { get; set; }
        public List<int> Adjustment { get; set; }
        public List<int> BudgetIP { get; set; }
        public List<decimal> Actual { get; set; }
        public List<decimal> To { get; set; }
    }
}
