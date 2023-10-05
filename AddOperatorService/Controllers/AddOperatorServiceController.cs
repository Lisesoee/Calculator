using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace AddOperatorService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AddOperatorServiceController : ControllerBase
    {
        private static IDbConnection addOperationsdb = new MySqlConnection("Server=addOperations-db;Database=addOperations-database;Uid=guest;Pwd=1234;");

        [HttpGet]
        [ActionName("GetResult")]
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
        [HttpGet]
        [ActionName("GetAllOperations")]
        public List<MathematicalOpearation> Get()
        {
            Console.WriteLine("Getting all add operations...");

            var operationList = new List<MathematicalOpearation>();

            var newOperation = new MathematicalOpearation
            {
                Id = 3,
                a = 23,
                b = 7,
                result = 30,
                MathematicOperator = "+"
            };
            operationList.Add(newOperation);

            var newOperation2 = new MathematicalOpearation
            {
                Id = 4,
                a = 15,
                b = 5,
                result = 20,
                MathematicOperator = "+"
            };
            operationList.Add(newOperation2);

            return operationList;

        }
    }
}