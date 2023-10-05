using Calculator.Models;
using Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calculator.Pages
{
    public class IndexModel : PageModel
    {       
        public double Result { get; set; }
        [BindProperty]
        public List<MathematicalOpearation> ListOfOperations { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var myCalculator = new CalculatorService();
            ListOfOperations = await myCalculator.GetListOfItemsAsync();

            return Page();
        }

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

        public async Task<IActionResult> OnPostListOfOperations()
        {
            var myCalculator = new CalculatorService();
            ListOfOperations = await myCalculator.GetListOfItemsAsync();

            return Page();
        }
    }
}