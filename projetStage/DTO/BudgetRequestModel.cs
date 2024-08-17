namespace projetStage.DTO
{
    public class BudgetRequestModel
    {
        public string Departement { get; set; }
        public List<int> InitialBudget { get; set; }
        public List<int> SalesBudget { get; set; }
        public List<int> SalesForecast { get; set; }
        public List<int> Adjustment { get; set; }
        public List<int> BudgetIP { get; set; }
    }

}
