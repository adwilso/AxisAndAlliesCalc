using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
namespace CalculatorUnitTests
{
    public class TestHelpers
    {
        public static Army CreateWithUnits(int count, bool includeAA)
        {
            return CreateWithTestUnits(count, includeAA, false, false);
        }
        public static Army CreateWithTestUnits(int count, bool includeAA, bool isTest, bool alwaysHit)
        {
            Army army = new Army();
            for (int i = 0; i < count; i++)
            {
                Unit temp;
                temp = new Infantry();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                army.Infantry.Add(temp);

                temp = new SupportedInfantry();                
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                army.SupportedInfantry.Add(temp);

                temp = new Artillery();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                army.Artillery.Add(temp);

                if (includeAA == true)
                {
                    temp = new AA();
                    temp.IsTest = isTest;
                    temp.AlwaysHit = alwaysHit;
                    army.AA.Add(temp);
                }

                temp = new Tank();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                army.Tanks.Add(temp);

                temp = new Fighter();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                army.Fighters.Add(temp);

                temp = new Bomber();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                army.Bombers.Add(temp);
            }
            return army;
        }
        public static Outcome CreateAttackerWin()
        {
            Army attacker = TestHelpers.CreateWithTestUnits(1, false, true, true);
            Army defender = TestHelpers.CreateWithTestUnits(1, true, true, false);
            return Outcome.Fight(attacker, defender);
        }
        public static Outcome CreateDefenderWin()
        {
            Army attacker = TestHelpers.CreateWithTestUnits(1, false, true, false);
            Army defender = TestHelpers.CreateWithTestUnits(1, true, true, true);
            return Outcome.Fight(attacker, defender);
        }
        public static Outcome CreateTie()
        {
            Army attacker = TestHelpers.CreateWithTestUnits(1, false, true, true);
            Army defender = TestHelpers.CreateWithTestUnits(1, false, true, true);
            return Outcome.Fight(attacker, defender);
        }
    }
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
            Assert.IsTrue(unit.doesHit(Posture.Defense));
        }
        [TestMethod]
        public void UnitSetup_UnitTestState_AlwaysMiss()
        {
            Unit unit = new Infantry();
            unit.AlwaysHit = false;
            unit.IsTest = true;
            Assert.IsFalse(unit.doesHit(Posture.Attack));
            Assert.IsFalse(unit.doesHit(Posture.Defense));
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
        [TestMethod]
        public void Army_AddUnitsAndCount_AllUnitTypes()
        {
            Army army = TestHelpers.CreateWithUnits(1, true);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 7);
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_Attacker()
        {
            Army army = TestHelpers.CreateWithUnits(1, false);
            army.RemoveGroundForceAttacker(4);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 2);
            Assert.IsTrue(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_OverflowAttacker()
        {
            Army army = TestHelpers.CreateWithUnits(1, false);
            army.RemoveGroundForceAttacker(999);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 0);
            Assert.IsFalse(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_AttackerNoRemoval()
        {
            Army army = TestHelpers.CreateWithUnits(1, false);
            int startingUnits = army.NumberOfRemainingUnits();
            army.RemoveGroundForceAttacker(0);
            Assert.AreEqual(army.NumberOfRemainingUnits(), startingUnits);
            Assert.IsTrue(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_Defender()
        {
            Army army = TestHelpers.CreateWithUnits(1, true);
            army.RemoveGroundForceDefender(6);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 1);
            Assert.IsTrue(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_OverflowDefending()
        {
            Army army = TestHelpers.CreateWithUnits(1, true);
            army.RemoveGroundForceDefender(999);
            Assert.AreEqual(army.NumberOfRemainingUnits(), 0);
            Assert.IsFalse(army.CanStillFight());
        }
        [TestMethod]
        public void Army_AddAndRemoveUnits_DefenderNoRemoval()
        {
            Army army = TestHelpers.CreateWithUnits(1, true);
            int startingUnits = army.NumberOfRemainingUnits();
            army.RemoveGroundForceDefender(0);
            Assert.AreEqual(army.NumberOfRemainingUnits(), startingUnits);
            Assert.IsTrue(army.CanStillFight());
        }
    }
    [TestClass]
    public class OutcomeUnitTests
    {
        [TestMethod]
        public void Outcome_FightFindWinner_AttackerWins()
        {
            Outcome outcome = TestHelpers.CreateAttackerWin();

            Assert.IsTrue(outcome.FinalAttacker.CanStillFight());
            Assert.IsFalse(outcome.FinalDefender.CanStillFight());
            Assert.AreEqual(outcome.FinalDefender.NumberOfRemainingUnits(), 0);
            Assert.AreNotEqual(outcome.FinalAttacker.NumberOfRemainingUnits(), 0);
        }
        [TestMethod]
        public void Outcome_FightFindWinner_DefenderWins()
        {
            Outcome outcome = TestHelpers.CreateDefenderWin();

            Assert.IsFalse(outcome.FinalAttacker.CanStillFight());
            Assert.IsTrue(outcome.FinalDefender.CanStillFight());
            Assert.AreNotEqual(outcome.FinalDefender.NumberOfRemainingUnits(), 0);
            Assert.AreEqual(outcome.FinalAttacker.NumberOfRemainingUnits(), 0);
        }
        [TestMethod]
        public void Outcome_FightFindWinner_Tie()
        {
            Outcome outcome = TestHelpers.CreateTie();

            Assert.IsFalse(outcome.FinalAttacker.CanStillFight());
            Assert.IsFalse(outcome.FinalDefender.CanStillFight());
            Assert.AreEqual(outcome.FinalDefender.NumberOfRemainingUnits(), 0);
            Assert.AreEqual(outcome.FinalAttacker.NumberOfRemainingUnits(), 0);
        }

    }
    [TestClass]
    public class ResultsTrackerUnitTests
    {
        [TestMethod]
        public void ResultsTracker_AttackerWinsAll()
        {
            ResultsTracker results = new ResultsTracker();
            results.Outcomes.Add(TestHelpers.CreateAttackerWin());
            results.Outcomes.Add(TestHelpers.CreateAttackerWin());
            results.Outcomes.Add(TestHelpers.CreateAttackerWin());

            Assert.AreEqual(results.AttackerWins, 3);
            Assert.AreEqual(results.AttackerIPCLost, 0);
            Assert.AreEqual(results.AttackerWinRate, 100);
            Assert.AreEqual(results.DefenderWinRate, 0);
            Assert.AreEqual(results.DefenderWins, 0);
            Assert.AreEqual(results.TieRate, 0.0);
            Assert.AreEqual(results.Ties, 0);
            Assert.AreEqual(results.TotalFights, 3);
        }
        [TestMethod]
        public void ResultsTracker_DefenderWinsAll()
        {
            ResultsTracker results = new ResultsTracker();
            results.Outcomes.Add(TestHelpers.CreateDefenderWin());
            results.Outcomes.Add(TestHelpers.CreateDefenderWin());
            results.Outcomes.Add(TestHelpers.CreateDefenderWin());

            Assert.AreEqual(results.AttackerWins, 0);
            Assert.AreEqual(results.DefenderIPCLost, 0);
            Assert.AreEqual(results.AttackerWinRate, 0.0);
            Assert.AreEqual(results.DefenderWinRate, 100);
            Assert.AreEqual(results.DefenderWins, 3);
            Assert.AreEqual(results.TieRate, 0.0);
            Assert.AreEqual(results.Ties, 0);
            Assert.AreEqual(results.TotalFights, 3);
        }
        [TestMethod]
        public void ResultsTracker_TieAll()
        {
            ResultsTracker results = new ResultsTracker();
            results.Outcomes.Add(TestHelpers.CreateTie());
            results.Outcomes.Add(TestHelpers.CreateTie());
            results.Outcomes.Add(TestHelpers.CreateTie());

            Assert.AreEqual(results.AttackerWins, 0);
            Assert.AreEqual(results.AttackerWinRate, 0.0);
            Assert.AreEqual(results.DefenderWinRate, 0.0);
            Assert.AreEqual(results.DefenderWins, 0);
            Assert.AreEqual(results.TieRate, 100);
            Assert.AreEqual(results.Ties, 3);
            Assert.AreEqual(results.TotalFights, 3);
        }
    }
}
