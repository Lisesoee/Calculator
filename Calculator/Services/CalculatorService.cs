using Calculator.Models;
using RestSharp;

namespace Calculator.Services
{
    public class CalculatorService
    {
        public double Add(double num1, double num2)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://addoperatorservice/AddOperatorService?a=" + num1 + "&b=" + num2);

            Console.WriteLine(client.BaseAddress);

            var response = client.Send(new HttpRequestMessage(HttpMethod.Get, ""));
            var stringTask = response.Content.ReadAsStringAsync();
            stringTask.Wait();

            Console.WriteLine(response.StatusCode + " from " + client.BaseAddress + " result: " + stringTask.Result);

            return double.Parse(stringTask.Result);
        }

        public double Subtract(double num1, double num2)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://subtractoperatorservice/SubtractOperatorService?a=" + num1 + "&b=" + num2);

            Console.WriteLine(client.BaseAddress);

            var response = client.Send(new HttpRequestMessage(HttpMethod.Get, ""));
            var stringTask = response.Content.ReadAsStringAsync();
            stringTask.Wait();

            Console.WriteLine(response.StatusCode + " from " + client.BaseAddress + " result: " + stringTask.Result);

            return double.Parse(stringTask.Result);
        }

        public List<MathematicalOpearation> GetListOfItems()
        //public async Task<List<string>> GetListOfItems()
        {
            Console.WriteLine("Fetching list...");
            var operationList = new List<MathematicalOpearation>();

            var newOperation = new MathematicalOpearation
            {
                Id = 1,
                a = 10,
                b = 5,
                result = 15,
                MathematicOperator = "+"
            };
            operationList.Add(newOperation);

            var newOperation2 = new MathematicalOpearation
            {
                Id = 2,
                a = 8,
                b = 2,
                result = 6,
                MathematicOperator = "-"
            };
            operationList.Add(newOperation2);

            // Fetch the list of items from a data source or perform any required operations
            // For demonstration purposes, we'll return a static list here
            return operationList;
        }
    }
}
