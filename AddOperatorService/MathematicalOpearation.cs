namespace AddOperatorService
{
    public class MathematicalOpearation
    {
        public MathematicalOpearation(int id, int a, int b, int result, string? mathematicOperator)
        {
            Id = id;
            this.a = a;
            this.b = b;
            this.result = result;
            MathematicOperator = mathematicOperator;
        }

        public int Id { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int result { get; set; }
        public string? MathematicOperator { get; set; }
    }    
}