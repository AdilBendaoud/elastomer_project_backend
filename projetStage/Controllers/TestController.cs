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
        public void Get() 
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=elastomer;Trusted_Connection=True;";
            string query = "SELECT FirstName, LastName FROM Persons";

            using (SqlConnection connection = new (connectionString))
            {
                SqlCommand command = new (query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["FirstName"]}, {reader["LastName"]}");
                    }
                }
            }
        }

        [HttpGet("hour")]
        public IActionResult GetHour()
        {

            return Ok(DateTime.Now);
        }
    }
}
