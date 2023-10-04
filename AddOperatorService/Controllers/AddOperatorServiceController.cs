using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace AddOperatorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddOperatorServiceController : ControllerBase
    {
        private static IDbConnection addOperationsdb = new MySqlConnection("Server=addOperations-db;Database=addOperations-database;Uid=guest;Pwd=1234;");

        [HttpGet]
        public int Get(int a, int b)
        {
            int result = a + b;
            Console.WriteLine(a + " + " + b + " = " + result);

            addOperationsdb.Open();
            var tables = addOperationsdb.Query<string>("SHOW TABLES LIKE 'addOperations'");
            if (!tables.Any())
            {
                addOperationsdb.Execute("CREATE TABLE addOperations (id INT AUTO_INCREMENT PRIMARY KEY, a INT NOT NULL, b INT NOT NULL, result INT NOT NULL)");
                Console.WriteLine("Table created");
            }

            addOperationsdb.Execute("INSERT INTO addOperations (a, b, result) VALUES (@a, @b, @result)", new { a = a, b = b, result = result });
            Console.WriteLine("Result stored in database");
            addOperationsdb.Close();           

            return result;
        }
    }
}