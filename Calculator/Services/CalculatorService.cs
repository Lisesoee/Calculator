using Calculator.Models;
using RestSharp;
using System;
using System.Net.Http;
using System.Text.Json;

namespace Calculator.Services
{

    public class CalculatorService
    {
        
        //public double Add(double num1, double num2)
        public async Task<double> Add(double num1, double num2)
        {
            RestClient restClient = new RestClient("http://addoperatorservice/AddOperatorService/");
            var task = restClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));

            Console.WriteLine("Retrived result from Add operation: " + task.Result);
            return task.Result;
        }

        public async Task<double> Subtract(double num1, double num2)
        {
            RestClient restClient = new RestClient("http://subtractoperatorservice/SubtractOperatorService/");
            var task = restClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));

            Console.WriteLine("Retrived result from Subtract operation: " + task.Result);
            return task.Result;
        }

        public async Task<List<MathematicalOpearation>> GetListOfItemsAsync()
        {
            Console.WriteLine("Fetching list...");

            var addOperationList = await GetListOfAddOperations();
            var subtractOperationList = await GetListOfSubtractOperations();

            var operationList = new List<MathematicalOpearation>();
            operationList.AddRange(addOperationList);
            operationList.AddRange(subtractOperationList);  

            return operationList;

        }
        private async Task<List<MathematicalOpearation>> GetListOfAddOperations()
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
                foreach (var operation in operationsList)
                {
                    operation.MathematicOperator = "+";
                }

                Console.WriteLine("Retrieved number of add operations: " + operationsList.Count);

                return operationsList;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                var operationList = new List<MathematicalOpearation>();
                return operationList;
            }
        }

        private async Task<List<MathematicalOpearation>> GetListOfSubtractOperations()
        {
            Console.WriteLine("Fetching list...");

            // Create an HttpClient instance
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://subtractoperatorservice/SubtractOperatorService/GetAllOperations");

            // Send an HTTP GET request
            using var response = await client.GetAsync("");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a list of MathematicalOperation objects
                var operationsList = JsonSerializer.Deserialize<List<MathematicalOpearation>>(responseContent);
                foreach (var operation in operationsList)
                {
                    operation.MathematicOperator = "-";
                }

                Console.WriteLine("Retrieved number of subtract operations: " + operationsList.Count);

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
