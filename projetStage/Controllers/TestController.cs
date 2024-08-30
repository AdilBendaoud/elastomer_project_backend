using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get() 
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=appDb;User Id=elastomer_app;Password=StrongPassword123;";
            
            string query = "SELECT CurrencyCode FROM WESM_currencies";
            List<string> myNames = new List<String>();

            try {
                using (SqlConnection connection = new(connectionString))
                {
                    SqlCommand command = new(query, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            myNames.Add(reader.GetString(0));
                        }
                    }
                }
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(myNames);
        }

        [HttpGet("hour")]
        public IActionResult GetHour()
        {

            return Ok(DateTime.Now);
        }
    }
}
