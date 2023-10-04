using Microsoft.AspNetCore.Mvc;

namespace AddOperatorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddOperatorServiceController : ControllerBase
    {
        [HttpGet]
        public int Get(int a, int b)
        {
            return a+b;
        }
    }
}