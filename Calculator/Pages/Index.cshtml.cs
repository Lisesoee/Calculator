using Calculator.Models;
using Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calculator.Pages
{
    public class IndexModel : PageModel
    {       
        public double Result { get; set; }
        public List<MathematicalOpearation> ListOfOperations { get; set; }

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

        public void OnPostListOfOperations()
        {
            var myCalculator = new CalculatorService();
            ListOfOperations = myCalculator.GetListOfItems();
        }
    }
}