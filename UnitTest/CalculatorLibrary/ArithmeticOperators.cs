using System;

namespace CalculatorLibrary
{
    public interface IArithmeticOperators
    {
        int Add(int first, int second);
        int Multiply(int first, int second);
    }

    public class ArithmeticOperators : IArithmeticOperators
    {
        public int Add(int first, int second)
        {
            return first + second;
        }

        public int Multiply(int first, int second)
        {
            return first + second;
        }
    }
}
