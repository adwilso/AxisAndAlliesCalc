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

    [TestClass]
    public class ArmyUnitTests
    {
        //Helper Methods

        private Army CreateWithUnits(int count, bool includeAA)
        {
            Army army = new Army();

            for (int i = 0; i < count; i++)
            {
                army.Infantry.Add(new Infantry());
                army.SupportedInfantry.Add(new SupportedInfantry());
                army.Artillery.Add(new Artillery());
                if (includeAA == true)
                {
                    army.AA.Add(new AA());
                }
                army.Tanks.Add(new Tank());
                army.Fighters.Add(new Fighter());
                army.Bombers.Add(new Bomber());
            }
            return army;
        }

        [TestMethod]
        public void Army_AddUnitsAndCount_AllUnitTypes()
        {
            Army army = CreateWithUnits(1, true);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 7);
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_Attacker()
        {
            Army army = CreateWithUnits(1, false);
            army.RemoveGroundForceAttacker(4);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 2);
            Assert.IsTrue(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_OverflowAttacker()
        {
            Army army = CreateWithUnits(1, false);
            army.RemoveGroundForceAttacker(999);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 0);
            Assert.IsFalse(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_AttackerNoRemoval()
        {
            Army army = CreateWithUnits(1, false);
            int startingUnits = army.NumberOfRemainingUnits();
            army.RemoveGroundForceAttacker(0);
            Assert.AreEqual(army.NumberOfRemainingUnits(), startingUnits);
            Assert.IsTrue(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_Defender()
        {
            Army army = CreateWithUnits(1, true);
            army.RemoveGroundForceDefender(6);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 1);
            Assert.IsTrue(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_OverflowDefending()
        {
            Army army = CreateWithUnits(1, true);
            army.RemoveGroundForceDefender(999);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 0);
            Assert.IsFalse(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_DefenderNoRemoval()
        {
            Army army = CreateWithUnits(1, true);
            int startingUnits = army.NumberOfRemainingUnits();
            army.RemoveGroundForceDefender(0);
            Assert.AreEqual(army.NumberOfRemainingUnits(), startingUnits);
            Assert.IsTrue(army.CanStillFight());
        }
    }
}
