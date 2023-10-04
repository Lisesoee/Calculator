namespace SubtractOperatorService
{
    public class Operation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int result { get; set; }
        public string? MathematicOperator { get; set; }
    }
}