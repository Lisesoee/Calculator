using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace SubtractOperatorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubtractOperatorServiceController : ControllerBase
    {
        private static IDbConnection subtractOperationsdb = new MySqlConnection("Server=subtractOperations-db;Database=subtractOperations-database;Uid=guest;Pwd=1234;");

        [HttpGet]
        public int Get(int a, int b)
        {
            var result = a - b;
            Console.WriteLine(a + " - " + b + " = " + result);

            subtractOperationsdb.Open();
            var tables = subtractOperationsdb.Query<string>("SHOW TABLES LIKE 'subtractOperations'");
            if (!tables.Any())
            {
                subtractOperationsdb.Execute("CREATE TABLE subtractOperations (id INT AUTO_INCREMENT PRIMARY KEY, a INT NOT NULL, b INT NOT NULL, result INT NOT NULL)");
                Console.WriteLine("Table created");
            }

            subtractOperationsdb.Execute("INSERT INTO subtractOperations (a, b, result) VALUES (@a, @b, @result)", new { a = a, b = b, result = result });
            Console.WriteLine("Result stored in database");
            subtractOperationsdb.Close();

            return result;
        }
    }
}