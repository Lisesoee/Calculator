using Calculator.Models;
using Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calculator.Pages
{
    public class IndexModel : PageModel
    {       
        public double AddResult { get; set; }
        public double SubtractResult { get; set; }
        [BindProperty]
        public List<MathematicalOpearation> ListOfOperations { get; set; }
        public async Task<IActionResult> OnGet()
        {
            // Runs when opening page
            var myCalculator = new CalculatorService();
            ListOfOperations = await myCalculator.GetListOfItemsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAdd(double num1, double num2)
        {
            var myCalculator = new CalculatorService();
            AddResult = await myCalculator.Add(num1, num2);

            return Page();
        }
        public async Task<IActionResult> OnPostSubtract(double num1, double num2)
        {
            var myCalculator = new CalculatorService();
            SubtractResult = await myCalculator.Subtract(num1, num2);

            return Page();
        }

        public async Task<IActionResult> OnPostListOfOperations()
        {
            // Runs when button is pressed
            var myCalculator = new CalculatorService();
            ListOfOperations = await myCalculator.GetListOfItemsAsync();

            return Page();
        }
    }
}