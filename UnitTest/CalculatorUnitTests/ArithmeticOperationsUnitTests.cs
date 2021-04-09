using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorLibrary;

namespace CalculatorUnitTests
{
    [TestClass]
    public class ArithmeticOperationsUnitTests
    {
        [TestMethod]
        public void AddPositiveNumbersTest()
        {
            var arithOperator = new ArithmeticOperators();
            int first = 10;
            int second = 20;
            int expectedResult = 30;

            int result = arithOperator.Add(first, second);

            Assert.AreEqual(expectedResult, result, "Add two positive numbers is ok");
        }

        [TestMethod]
        public void AddNegativeNumbersTest()
        {
            var arithOperator = new ArithmeticOperators();
            int first = 10;
            int second = -20;
            int expectedResult = -10;

            int result = arithOperator.Add(first, second);

            Assert.AreEqual(expectedResult, result, "Add one positive and one negative numbers is ok");
        }

        [TestMethod]
        public void AddOppositeNumbersTest()
        {
            var arithOperator = new ArithmeticOperators();
            int first = -10;
            int second = -20;
            int expectedResult = -10;

            int result = arithOperator.Add(first, second);

            Assert.AreEqual(expectedResult, result, "Add two negative numbers is ok");
        }
    }
}
