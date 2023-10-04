using RestSharp;

namespace Calculator.Services
{
    public class CalculatorService
    {
        //private static RestClient AddOperatorRestClient = new RestClient("http://AddOperatorService/");
        //private static RestClient SubtractOperatorRestClient = new RestClient("http://SubtractOperatorService/");

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
            //return num1 + num2;
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
            //return num1 - num2;
        }
    }
}
