namespace Calculator
{
    using System.Linq;
    using Calculator.Service;

    internal static class Program
    {
        private static readonly DatabaseService DatabaseService = new DatabaseService();
        private static readonly CalculatorService CalculatorService = new CalculatorService(DatabaseService);

        private static void Main(string[] args)
        {
            CalculatorService.Calculate(args);
        }
    }
}