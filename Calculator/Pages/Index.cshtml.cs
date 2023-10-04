using Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calculator.Pages
{
    public class IndexModel : PageModel
    {       

        public double Result { get; set; }

        public void OnPostAdd(double num1, double num2)
        {
            var myCalculator = new CalculatorService();
            Result = myCalculator.Add(num1, num2);
        }

        public void OnPostSubtract(double num1, double num2)
        {
            var myCalculator = new CalculatorService();
            Result = myCalculator.Subtract(num1, num2);
        }

        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        //public void OnGet()
        //{

        //}
        //public IActionResult OnPostAdd(double num1, double num2)
        //{
        //    double result = num1 + num2;
        //    return new JsonResult(result);
        //}

        //public IActionResult OnPostSubtract(double num1, double num2)
        //{
        //    double result = num1 - num2;
        //    return new JsonResult(result);
        //}

        //public async Task<IActionResult> OnPostAdd(double num1, double num2)
        //{
        //    double result = await _calculatorService.Add(num1, num2);
        //    return new JsonResult(result);
        //}

        //public async Task<IActionResult> OnPostSubtract(double num1, double num2)
        //{
        //    double result = await _calculatorService.Subtract(num1, num2);
        //    return new JsonResult(result);
        //}

    }
}