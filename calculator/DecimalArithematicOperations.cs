using System;

namespace calculator
{
    public class DecimalArithmeticOperations : IArithematicOperations<decimal>
    {
        public decimal Add(decimal a, decimal b) => a + b;
        public decimal Subtract(decimal a, decimal b) => a - b;
        public decimal Multiply(decimal a, decimal b) => a * b;

        public decimal Divide(decimal a, decimal b)
        {
            if (b == 0) throw new DivideByZeroException("Division by zero is not allowed.");
            return a / b;
        }
    }
}
