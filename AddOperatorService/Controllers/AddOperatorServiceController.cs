using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
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
            RollTheDice(3);           

            int result = a + b;
            Console.WriteLine(a + " + " + b + " = " + result);

            addOperationsdb.Open();
            var tables = addOperationsdb.Query<string>("SHOW TABLES LIKE 'addOperations'");
            if (!tables.Any())
            {
                addOperationsdb.Execute("CREATE TABLE addOperations (id INT AUTO_INCREMENT PRIMARY KEY, a INT NOT NULL, b INT NOT NULL, result INT NOT NULL, MathematicalOperator ENUM('+', '-') NOT NULL)");
                Console.WriteLine("Table created");
            }

            addOperationsdb.Execute("INSERT INTO addOperations (a, b, result) VALUES (@a, @b, @result)", new { a = a, b = b, result = result });
            Console.WriteLine("Result stored in database");
            addOperationsdb.Close(); 
            
            //Thread.Sleep(2000);

            return result;
        }

        private void RollTheDice(int x)
        {
            // Simulate 1/x chance of failure
            Random rand = new Random();
            bool randomBoolean = rand.Next(x - 1) != 0;
            if (randomBoolean)
            {
                Console.WriteLine("The dice was not with you. Error when adding!");
                throw new Exception();
            }
        }

        [HttpGet]
        [ActionName("GetAllOperations")]
        public List<MathematicalOpearation> Get()
        {
            //RollTheDice(5); 

            Console.WriteLine("Getting all add operations...");

            addOperationsdb.Open();

            var tables = addOperationsdb.Query<string>("SHOW TABLES LIKE 'addOperations'");
            if (!tables.Any())
            {
                addOperationsdb.Execute("CREATE TABLE addOperations (id INT AUTO_INCREMENT PRIMARY KEY, a INT NOT NULL, b INT NOT NULL, result INT NOT NULL, MathematicalOperator ENUM('+', '-') NOT NULL)");
                Console.WriteLine("Table created");
            }

            List<MathematicalOpearation> operationList = new List<MathematicalOpearation>();

            MySqlCommand cmd = addOperationsdb.CreateCommand() as MySqlCommand;
            cmd.CommandText = "SELECT * FROM addOperations";
            MySqlDataReader reader = cmd.ExecuteReader() as MySqlDataReader;
            while (reader.Read())
            {
                Console.WriteLine("Read something... ");

                int id = reader.GetInt32(0);
                int a = reader.GetInt32(1);
                int b = reader.GetInt32(2);
                int result = reader.GetInt32(3);
                string mathematicOperator = "+";// reader.GetString(4);

                MathematicalOpearation newOperation = new MathematicalOpearation(id, a, b, result, mathematicOperator);
                operationList.Add(newOperation);
            }

            Console.WriteLine("Finished loading list.");
            addOperationsdb.Close();

            //Thread.Sleep(1000);

            return operationList;
        }
    }
}