using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calculator.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        //public void OnGet()
        //{

        //}
        public IActionResult OnPostAdd(double num1, double num2)
        {
            double result = num1 + num2;
            return new JsonResult(result);
        }

        public IActionResult OnPostSubtract(double num1, double num2)
        {
            double result = num1 - num2;
            return new JsonResult(result);
        }

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