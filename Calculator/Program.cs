namespace Calculator
{
    using System;
    using System.Linq;
    using Calculator.Service;

    internal class Program
    {
        private static readonly DatabaseService DatabaseService = new DatabaseService();
        private static readonly CalculatorService CalculatorService = new CalculatorService();

        private static void Main(string[] args)
        {
            var summoners = DatabaseService.GetAll().ToList();
            CalculatorService.Calculate(args, summoners);
        }
    }
}