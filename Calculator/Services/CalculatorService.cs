using Calculator.Models;
using RestSharp;
using System;
using System.Net.Http;
using System.Text.Json;
using Polly;
using System.Transactions;
using System.Threading.Tasks;
using System.Threading;

namespace Calculator.Services
{

    public class CalculatorService
    {
        private static RestClient addOperationRestClient = new RestClient("http://addoperatorservice/AddOperatorService/");
        private static RestClient subtractOperationRestClient = new RestClient("http://subtractoperatorservice/SubtractOperatorService/");
        public async Task<double> Add(double num1, double num2)
        {
            //var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));

            //Console.WriteLine("Retrived result from Add operation: " + task.Result);
            //return task.Result;


            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(2, retryAttempt =>
                {
                    Console.WriteLine("Attempt... " + retryAttempt);
                    var timeToRetry = TimeSpan.FromSeconds(2);
                    Console.WriteLine($"Waiting {timeToRetry.TotalSeconds} seconds");
                    return timeToRetry;
                });

            try
            {
                return await retryPolicy.ExecuteAsync(async () =>
                {
                    //var response = await httpClient.GetAsync("https://urlnotexist.com/api/products/1");
                    //response.EnsureSuccessStatusCode();

                    var response = await AddAsync(num1, num2);

                    //return ((long)response);
                    return response;
                    

                    Console.WriteLine("Attempting to get added result...");
                    var task = AddAsync(num1, num2);
                    await task;
                    return task.Result;

                    //var task = await AddAsync(num1, num2);
                    //return task;//.Result;


                    //await AddAsync(num1, num2);

                    //var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));
                    ////await task.RunSynchronously();

                    //if (task?.Status == TaskStatus.RanToCompletion)
                    //{
                    //    Console.WriteLine("Retrived result from Add operation: " + task.Result);
                    //    return task.Result;
                    //}

                    //throw new Exception("Request failed. Task status: " + task?.Status);
                });
            }
            catch (Exception ex)
            {
                
                //Console.WriteLine("Final Throw");     
                Console.WriteLine("Finbal exeption: " + ex.ToString());
                return 8888888888;
            }


            // This code is to avoid "not all code paths return a value", but is not meant to actually be called...
            return 999999999;

            //await AddAsync(num1, num2);
            //var task = AddAsync(num1, num2);
            //return task.Result;

            //var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));
            //if (task?.Status == TaskStatus.RanToCompletion)
            //{
            //    Console.WriteLine("Retrived result from Add operation: " + task.Result);
            //    return task.Result;
            //}
            //throw new Exception("Request failed. Task status: " + task?.Status);


            //policy.Execute(() =>
            //{
            //    //var task = restClient.GetAsync<int>(new RestRequest("/cache?number=" + number));
            //    //await task;

            //    Console.WriteLine("Attempting to get added result...");
            //    var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));


            //    if (task?.Status == TaskStatus.RanToCompletion)
            //    {
            //        Console.WriteLine("Retrived result from Add operation: " + task.Result);
            //        return task.Result;
            //    }

            //    throw new Exception("Request failed. Task status: " + task?.Status);
            //});

            //var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));
            //return task.Result;
            //return 0;

            //});

        }

        public async Task<double> AddAsync(double num1, double num2)
        {
            //var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));

            //Console.WriteLine("Retrived result from Add operation: " + task.Result);
            //return task.Result;

            var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));
            await task;
            //task.Wait();

            if (task?.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine("Retrived result from Add operation: " + task.Result);
                return task.Result;
            }

            if (task?.Status == TaskStatus.Faulted)
            {
                throw new Exception("Request failed. Task status: " + task?.Status);                
            }

            Console.WriteLine("Task status: " + task?.Status);

            return 7777777777;
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

            //operationList.Sort();

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
