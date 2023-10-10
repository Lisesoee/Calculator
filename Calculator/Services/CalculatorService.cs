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
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("Started method Add({num1},{num2})", num1, num2);

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt =>
                {
                    Console.WriteLine("Retry attempt... " + retryAttempt);
                    var timeToRetry = TimeSpan.FromSeconds(1);
                    Console.WriteLine($"Waiting {timeToRetry.TotalSeconds} seconds before next retry");
                    MonitorService.Log.Warning("Retry attempt {retryAttempt} after {timeToRetry} seconds)", retryAttempt, timeToRetry.TotalSeconds);
                    return timeToRetry;
                });
            try
            {
                return await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var actCreateTable = MonitorService.ActivitySource.StartActivity("TryGetResult"))
                    {
                        var task = addOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));
                        await task;

                        if (task?.Status == TaskStatus.RanToCompletion)
                        {
                            MonitorService.Log.Debug("Retrived result from Add operation: {result}", task.Result);
                            Console.WriteLine("Retrived result from Add operation: " + task.Result);
                            return task.Result;
                        }
                        if (task?.Status == TaskStatus.Faulted)
                        {
                            MonitorService.Log.Error("Request failed. Task status: {Status}", task.Status);
                            throw new Exception("Request failed. Task status: " + task?.Status);
                        }
                        MonitorService.Log.Error("Unexpected Task status: {Status}", task.Status);
                        throw new Exception("Unexpected Task status: " + task?.Status);
                    }
                });
            }
            catch (Exception ex)
            {
                MonitorService.Log.Error("Ran out of retries. Final exception: {exception}", ex.ToString);
                Console.WriteLine("Ran out of retries. Final exception: " + ex.ToString());
                return 0;
            }
        }

        public async Task<double> Subtract(double num1, double num2)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt =>
                {
                    Console.WriteLine("Retry attempt... " + retryAttempt);
                    var timeToRetry = TimeSpan.FromSeconds(1);
                    Console.WriteLine($"Waiting {timeToRetry.TotalSeconds} seconds before next retry");
                    MonitorService.Log.Warning("Retry attempt {retryAttempt} after {timeToRetry} seconds)", retryAttempt, timeToRetry.TotalSeconds);
                    return timeToRetry;
                });
            try
            {
                return await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var actCreateTable = MonitorService.ActivitySource.StartActivity("TryGetResult"))
                    {
                        var task = subtractOperationRestClient.GetAsync<int>(new RestRequest("/GetResult?a=" + num1 + "&b=" + num2));
                        await task;

                        if (task?.Status == TaskStatus.RanToCompletion)
                        {
                            MonitorService.Log.Debug("Retrived result from Subtract operation: {Result}", task.Result);
                            Console.WriteLine("Retrived result from Subtract operation: " + task.Result);
                            return task.Result;
                        }
                        if (task?.Status == TaskStatus.Faulted)
                        {
                            MonitorService.Log.Error("Request failed. Task status: {Status}", task.Status);
                            throw new Exception("Request failed. Task status: " + task?.Status);
                        }
                        MonitorService.Log.Error("Unexpected Task status: {Status}", task.Status);
                        throw new Exception("Unexpected Task status: " + task?.Status);
                    }

                });
            }
            catch (Exception ex)
            {
                MonitorService.Log.Error("Ran out of retries. Final exception: {exception}", ex.ToString);
                return 0;
            }
        }

        public async Task<List<MathematicalOpearation>> GetListOfItemsAsync()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("Fetching list of all operations...");
            Console.WriteLine("Fetching list of all operations...");

            var addOperationList = await GetListOfAddOperations();
            var subtractOperationList = await GetListOfSubtractOperations();

            var operationList = new List<MathematicalOpearation>();
            operationList.AddRange(addOperationList);
            operationList.AddRange(subtractOperationList);

            return operationList;
        }

        private async Task<List<MathematicalOpearation>> GetListOfAddOperations()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Console.WriteLine("Fetching list of add operations...");

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(2, retryAttempt =>
                {
                    Console.WriteLine("Retry attempt... " + retryAttempt);
                    var timeToRetry = TimeSpan.FromSeconds(1);
                    Console.WriteLine($"Waiting {timeToRetry.TotalSeconds} seconds before next retry");
                    MonitorService.Log.Warning("Retry attempt {retryAttempt} after {timeToRetry} seconds)", retryAttempt, timeToRetry.TotalSeconds);
                    return timeToRetry;
                });
            try
            {
                return await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var actCreateTable = MonitorService.ActivitySource.StartActivity("TryGetAllOperations"))
                    {
                        var task = addOperationRestClient.GetAsync<List<MathematicalOpearation>>(new RestRequest("/GetAllOperations"));
                        await task;

                        if (task?.Status == TaskStatus.RanToCompletion)
                        {
                            MonitorService.Log.Debug("Retrieved number of add operations: {Count}", task.Result.Count);
                            Console.WriteLine("Retrieved number of add operations: " + task.Result.Count);
                            return task.Result;
                        }
                        if (task?.Status == TaskStatus.Faulted)
                        {
                            MonitorService.Log.Error("Request failed. Task status: {Status}", task.Status);
                            throw new Exception("Request failed. Task status: " + task?.Status);
                        }
                        MonitorService.Log.Error("Unexpected Task status: {task.Status}", task.Status);
                        throw new Exception("Unexpected Task status: " + task?.Status);
                    }

                });
            }
            catch (Exception ex)
            {
                MonitorService.Log.Error("Ran out of retries. Final exception: {exception}", ex.ToString);
                var emptyOperationsList = new List<MathematicalOpearation>();
                return emptyOperationsList;
            }
        }

        private async Task<List<MathematicalOpearation>> GetListOfSubtractOperations()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Console.WriteLine("Fetching list of subtract operations...");

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(2, retryAttempt =>
                {
                    Console.WriteLine("Retry attempt... " + retryAttempt);
                    var timeToRetry = TimeSpan.FromSeconds(1);
                    Console.WriteLine($"Waiting {timeToRetry.TotalSeconds} seconds before next retry");
                    MonitorService.Log.Warning("Retry attempt {retryAttempt} after {timeToRetry} seconds)", retryAttempt, timeToRetry.TotalSeconds);
                    return timeToRetry;
                });
            try
            {
                return await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var actCreateTable = MonitorService.ActivitySource.StartActivity("TryGetAllOperations"))
                    {
                        var task = subtractOperationRestClient.GetAsync<List<MathematicalOpearation>>(new RestRequest("/GetAllOperations"));
                        await task;

                        if (task?.Status == TaskStatus.RanToCompletion)
                        {
                            MonitorService.Log.Debug("Retrieved number of subtract operations: {Count}", task.Result.Count);
                            Console.WriteLine("Retrieved number of subtract operations: " + task.Result.Count);
                            return task.Result;
                        }
                        if (task?.Status == TaskStatus.Faulted)
                        {
                            MonitorService.Log.Error("Request failed. Task status: {Status}", task.Status);
                            throw new Exception("Request failed. Task status: " + task?.Status);
                        }
                        MonitorService.Log.Error("Unexpected Task status: {Status}", task.Status);
                        throw new Exception("Unexpected Task status: " + task?.Status);
                    }

                });
            }
            catch (Exception ex)
            {
                MonitorService.Log.Error("Ran out of retries. Final exception: {exception}", ex.ToString);
                var emptyOperationsList = new List<MathematicalOpearation>();
                return emptyOperationsList;
            }
        }
    }
}
