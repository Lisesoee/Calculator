using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace SubtractOperatorService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SubtractOperatorServiceController : ControllerBase
    {
        private static IDbConnection subtractOperationsdb = new MySqlConnection("Server=subtractOperations-db;Database=subtractOperations-database;Uid=guest;Pwd=1234;");

        [HttpGet]
        [ActionName("GetResult")]
        public int Get(int a, int b)
        {
            RollTheDice(7);

            var result = a - b;
            Console.WriteLine(a + " - " + b + " = " + result);

            subtractOperationsdb.Open();
            var tables = subtractOperationsdb.Query<string>("SHOW TABLES LIKE 'subtractOperations'");
            if (!tables.Any())
            {
                subtractOperationsdb.Execute("CREATE TABLE subtractOperations (id INT AUTO_INCREMENT PRIMARY KEY, a INT NOT NULL, b INT NOT NULL, result INT NOT NULL, MathematicalOperator ENUM('+', '-') NOT NULL))");
                Console.WriteLine("Table created");
            }

            subtractOperationsdb.Execute("INSERT INTO subtractOperations (a, b, result) VALUES (@a, @b, @result)", new { a = a, b = b, result = result });
            Console.WriteLine("Result stored in database");
            subtractOperationsdb.Close();

            return result;
        }
        private void RollTheDice(int x)
        {
            // Simulate 1/x chance of failure
            Random rand = new Random();
            bool randomBoolean = rand.Next(x - 1) != 0;
            if (randomBoolean) 
            {
                Console.WriteLine("The dice was not with you!");
                throw new Exception(); 
            }
        }

        [HttpGet]
        [ActionName("GetAllOperations")]
        public List<MathematicalOperation> Get()
        {
            RollTheDice(10);

            Console.WriteLine("Getting all subtract operations...");

            subtractOperationsdb.Open();

            var tables = subtractOperationsdb.Query<string>("SHOW TABLES LIKE 'subtractOperations'");
            if (!tables.Any())
            {
                subtractOperationsdb.Execute("CREATE TABLE subtractOperations (id INT AUTO_INCREMENT PRIMARY KEY, a INT NOT NULL, b INT NOT NULL, result INT NOT NULL, MathematicalOperator ENUM('+', '-') NOT NULL)");
                Console.WriteLine("Table created");
            }

            List<MathematicalOperation> operationList = new List<MathematicalOperation>();

            MySqlCommand cmd = subtractOperationsdb.CreateCommand() as MySqlCommand;
            cmd.CommandText = "SELECT * FROM subtractOperations";
            MySqlDataReader reader = cmd.ExecuteReader() as MySqlDataReader;
            while (reader.Read())
            {
                Console.WriteLine("Read something... ");

                int id = reader.GetInt32(0);
                int a = reader.GetInt32(1);
                int b = reader.GetInt32(2);
                int result = reader.GetInt32(3);
                string mathematicOperator = "-";// reader.GetString(4);

                MathematicalOperation newOperation = new MathematicalOperation(id, a, b, result, mathematicOperator);
                operationList.Add(newOperation);
            }

            Console.WriteLine("Finished loading list.");
            subtractOperationsdb.Close();

            return operationList;
        }
    }
}