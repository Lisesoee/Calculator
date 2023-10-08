using Calculator.Models;
using RestSharp;
using System;
using System.Net.Http;
using System.Text.Json;

namespace Calculator.Services
{

    public class CalculatorService
    {
        private static RestClient addOperationRestClient = new RestClient("http://addoperatorservice/AddOperatorService/");
        private static RestClient subtractOperationRestClient = new RestClient("http://subtractoperatorservice/SubtractOperatorService/");
        public async Task<double> Add(double num1, double num2)
        {
            var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));

            Console.WriteLine("Retrived result from Add operation: " + task.Result);
            return task.Result;
        }

        public async Task<double> Subtract(double num1, double num2)
        {
            var task = subtractOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));

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
            Console.WriteLine("Fetching list of add operations...");

            var task = addOperationRestClient.GetAsync<List<MathematicalOpearation>>(new RestRequest("/GetAllOperations"));

            Console.WriteLine("Retrieved number of add operations: " + task.Result.Count);
            return task.Result;
        }

        private async Task<List<MathematicalOpearation>> GetListOfSubtractOperations()
        {
            Console.WriteLine("Fetching list of subtract operations...");

            var task = subtractOperationRestClient.GetAsync<List<MathematicalOpearation>>(new RestRequest("/GetAllOperations"));

            Console.WriteLine("Retrieved number of subtract operations: " + task.Result.Count);
            return task.Result;
        }
    }
}
