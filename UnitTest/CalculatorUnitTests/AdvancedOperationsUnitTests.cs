using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorLibrary;
using System;
using Moq;

namespace CalculatorUnitTests
{
    [TestClass]
    public class AdvancedOperationsUnitTests
    {
        
        [TestMethod]
        public void ExponentNumberTest()
        {
            var advancedOperations = new AdvancedOperations();
            int baseNumber = 10;
            int power = 2;
            int expectedResult = 100;

            int result = advancedOperations.Exponential(baseNumber, power);

            Assert.AreEqual(expectedResult, result, "Exponent of a number");
        }

        [TestMethod]
        public void ExponentNumberAdvancedTest()
        {
            var advancedOperations = new AdvancedOperations();
            var arithmetic = new Mock<IArithmeticOperators>();

            arithmetic.Setup(r => r.Multiply(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((Func<int,int,int>)((a,b) => a*b));
            int baseNumber = 10;
            int power = 4;
            int expectedResult = 10000;

            int result = advancedOperations.ExponentialAdvanced(arithmetic.Object, baseNumber, power);

            Assert.AreEqual(expectedResult, result, "Exponent of a number");
        }

        [TestMethod]
        public void ExponentNumberPower0Test()
        {
            var advancedOperations = new AdvancedOperations();
            int baseNumber = 10;
            int power = 0;
            int expectedResult = 1;

            int result = advancedOperations.Exponential(baseNumber, power);

            Assert.AreEqual(expectedResult, result, "Exponent of a number");
        }

        [TestMethod]
        public void ExponentNumberNegativePowerTest()
        {
            var advancedOperations = new AdvancedOperations();
            int baseNumber = 10;
            int power = -1;

            Assert.ThrowsException<ArgumentException>(() => advancedOperations.Exponential(baseNumber, power));
        }
    }
}