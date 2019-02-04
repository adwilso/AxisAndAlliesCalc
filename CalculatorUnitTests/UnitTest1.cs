using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
namespace CalculatorUnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void UnitSetup_UnitTestState_AlwaysHit()
        {
            Unit unit = new Infantry();
            unit.AlwaysHit = true;
            unit.IsTest = true;
            Assert.IsTrue(unit.doesHit(Posture.Attack));
        }

        [TestMethod]
        public void UnitSetup_UnitTestState_AlwaysMiss()
        {
            Unit unit = new Infantry();
            unit.AlwaysHit = false;
            unit.IsTest = true;
            Assert.IsFalse(unit.doesHit(Posture.Attack));
        }

        [TestMethod]
        public void UnitSetup_TestDetector_IsInTest()
        {
            Assert.IsTrue(UnitTestDetector.IsInUnitTest,
                "Should detect that we are running inside a unit test."); // lol
        }
    }
}
