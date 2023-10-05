using Calculator.Models;
using RestSharp;
using System.Net.Http;
using System.Text.Json;

namespace Calculator.Services
{
    public class CalculatorService
    {
        public double Add(double num1, double num2)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://addoperatorservice/AddOperatorService/GetResult?a=" + num1 + "&b=" + num2);

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

        public async Task<List<MathematicalOpearation>> GetListOfItemsAsync()
        //public async Task<List<string>> GetListOfItems()
        {
            Console.WriteLine("Fetching list...");

            // Create an HttpClient instance
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://addoperatorservice/AddOperatorService/GetAllOperations");

            // Send an HTTP GET request
            using var response = await client.GetAsync("");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a list of MathematicalOperation objects
                var operationsList = JsonSerializer.Deserialize<List<MathematicalOpearation>>(responseContent);


                Console.WriteLine("Retried number of operations: " + operationsList.Count);
                // Now you have the list of objects in the operationsList variable

                return operationsList;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                var operationList = new List<MathematicalOpearation>();
                return operationList;
            }

        }
    }
}
