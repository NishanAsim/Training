using System;

namespace CalculatorLibrary
{
    public class AdvancedOperations
    {
        public int Exponential(int baseNumber, int index)
        {
            if(index < 0)
            {
                throw new ArgumentException(nameof(index), "Power should not be -ve");
            }

            ArithmeticOperators operators = new ArithmeticOperators();
            int result = 1;
            for (int i = 0; i < index;i++)
            {
                result = operators.Multiply(result, baseNumber);
            }

            return result;
        }
    }
}