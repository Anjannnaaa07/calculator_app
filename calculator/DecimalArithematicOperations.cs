using System;

namespace calculator
{
    public class DecimalArithmeticOperations : IArithematicOperations<decimal>
    {
        public decimal Add(decimal a, decimal b) => checked(a + b);
        public decimal Subtract(decimal a, decimal b) => checked(a - b);
        public decimal Multiply(decimal a, decimal b) => checked(a * b);

        public decimal Divide(decimal a, decimal b)
        {
            if (b == 0)
                throw new DivideByZeroException("Division by zero is not allowed.");

            return checked(a / b);
        }

        public decimal ParseAndValidate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be empty.");

            if (!decimal.TryParse(input, out decimal value))
                throw new ArgumentException($"Invalid input: '{input}' is not a number.");

            return value;
        }
    }
}
