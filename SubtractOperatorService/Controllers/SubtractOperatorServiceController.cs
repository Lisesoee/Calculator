using Microsoft.AspNetCore.Mvc;

namespace SubtractOperatorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubtractOperatorServiceController : ControllerBase
    {
        [HttpGet]
        public int Get(int a, int b)
        {
            var result = a - b;
            Console.WriteLine(a + " - " + b + " = " + result);

            return result;
        }
    }
}