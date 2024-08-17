using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.Models;
using System.Data.SqlClient;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BudgetController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetBudget([FromQuery] string departement)
        {
            if (string.IsNullOrEmpty(departement))
            {
                return BadRequest("Department is required.");
            }

            var budgets = await _context.Budgets
                .Where(b => b.Departement == departement && b.Year == DateTime.Now.Year)
                .OrderBy(b => b.Month)
                .ToListAsync();

            string connectionString = _configuration.GetSection("ConnectionStrings")["SqlServerConnectionString"];
            
            string query = "EXEC dbo.SP_AP_ESM_Purchase_report;";
            List<ActualBudgetModel> allData = new List<ActualBudgetModel>();

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ActualBudgetModel data = new ActualBudgetModel
                        {
                            DateCreation = reader.GetDateTime(0),
                            Departement = reader.GetString(3),
                            Total = reader.GetDecimal(5)
                        };
                        allData.Add(data);
                    }
                }
            }

            string query2 = $"EXEC dbo.sp_Fact_DetAno_ESM '{DateTime.Now.Year}';";
            List<TOBudgetModel> allToData = new List<TOBudgetModel>();

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query2, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TOBudgetModel data = new TOBudgetModel
                        {
                           Month = reader.GetInt32(1),
                           Total = reader.GetDecimal(11)
                        };
                        allToData.Add(data);
                    }
                }
            }


            var filteredActualData = allData.Where(d => d.DateCreation.Year == DateTime.Now.Year && d.Departement == departement).ToList();
            var filteredToData = allToData.
                                    GroupBy(d => d.Month)
                                    .Select(g => new
                                    {
                                        Month = g.Key,
                                        Total = g.Sum(d => d.Total)
                                    });

            var monthlyActualSummaries = new decimal[12];

            foreach (var item in filteredActualData)
            {
                int monthIndex = item.DateCreation.Month - 1;
                monthlyActualSummaries[monthIndex] += item.Total;
            }

            var monthlyToSummaries = new decimal[12];

            foreach (var item in filteredToData)
            {
                int monthIndex = item.Month - 1;
                monthlyToSummaries[monthIndex] += item.Total;
            }

            var response = new BudgetResponse
            {
                InitialBudget = FillWithZeros(budgets.Select(b => b.InitialBudget)),
                SalesBudget = FillWithZeros(budgets.Select(b => b.SalesBudget)),
                SalesForecast = FillWithZeros(budgets.Select(b => b.SalesForecast)),
                Adjustment = FillWithZeros(budgets.Select(b => b.Adjustment)),
                BudgetIP = FillWithZeros(budgets.Select(b => b.BudgetIP)),
                Actual = monthlyActualSummaries.ToList(),
                To = monthlyToSummaries.ToList()
            };

            return Ok(response);
        }

        private List<int> FillWithZeros(IEnumerable<int?> values)
        {
            return values
                .Where(v => v.HasValue)
                .Select(v => v.Value)
                .Take(12)
                .Concat(Enumerable.Repeat(0, 12))
                .Take(12)
                .ToList();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBudget([FromBody] BudgetRequestModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Departement))
            {
                return BadRequest("Invalid data provided.");
            }

            for (int i = 0; i < 12; i++)
            {
                // Retrieve the existing budget entry if it exists
                var existingBudget = await _context.Budgets
                    .FirstOrDefaultAsync(b => b.Departement == model.Departement &&
                                              b.Month == i + 1 &&
                                              b.Year == DateTime.Now.Year);

                if (existingBudget != null)
                {
                    // Update existing budget entry
                    existingBudget.InitialBudget = model.InitialBudget.ElementAtOrDefault(i);
                    existingBudget.SalesBudget = model.SalesBudget.ElementAtOrDefault(i);
                    existingBudget.SalesForecast = model.SalesForecast.ElementAtOrDefault(i);
                    existingBudget.Adjustment = model.Adjustment.ElementAtOrDefault(i);
                    existingBudget.BudgetIP = model.BudgetIP.ElementAtOrDefault(i);
                }
                else
                {
                    // Create a new budget entry
                    var newBudget = new Budget
                    {
                        Departement = model.Departement,
                        Month = i + 1,
                        Year = DateTime.Now.Year,
                        InitialBudget = model.InitialBudget.ElementAtOrDefault(i),
                        SalesBudget = model.SalesBudget.ElementAtOrDefault(i),
                        SalesForecast = model.SalesForecast.ElementAtOrDefault(i),
                        Adjustment = model.Adjustment.ElementAtOrDefault(i),
                        BudgetIP = model.BudgetIP.ElementAtOrDefault(i),
                    };
                    _context.Budgets.Add(newBudget);
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Budget data saved or updated successfully.");
        }
    }
}
