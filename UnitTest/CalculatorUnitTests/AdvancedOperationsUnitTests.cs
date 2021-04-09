using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorLibrary;
using System;

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
            int power = 3;
            int expectedResult = 1000;

            int result = advancedOperations.Exponential(baseNumber, power);

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