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
            army.AddInfantry(count,isTest,alwaysHit);
            army.AddSupportedInfantry(count, isTest, alwaysHit);
            army.AddArtillery(count, isTest, alwaysHit);
            if (includeAA == true)
            {
                army.AddAA(count, isTest, alwaysHit);
            }
            army.AddTanks(count, isTest, alwaysHit);
            army.AddFighters(count, isTest, alwaysHit);
            army.AddBombers(count, isTest, alwaysHit);

            return army;
        }
        public static IOutcome CreateAttackerWin()
        {
            Army attacker = TestHelpers.CreateWithTestUnits(1, false, true, true);
            Army defender = TestHelpers.CreateWithTestUnits(1, true, true, false);
            return ArmyOutcome.Fight(attacker, defender);
        }
        public static IOutcome CreateDefenderWin()
        {
            Army attacker = TestHelpers.CreateWithTestUnits(1, false, true, false);
            Army defender = TestHelpers.CreateWithTestUnits(1, true, true, true);
            return ArmyOutcome.Fight(attacker, defender);
        }
        public static IOutcome CreateTie()
        {
            Army attacker = TestHelpers.CreateWithTestUnits(1, false, true, true);
            Army defender = TestHelpers.CreateWithTestUnits(1, false, true, true);
            return ArmyOutcome.Fight(attacker, defender);
        }
    }
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void UnitSetup_UnitTestState_AlwaysHit()
        {
            Unit unit = new Infantry(true, true);
            Assert.IsTrue(unit.doesHit(Posture.Attack));
            Assert.IsTrue(unit.doesHit(Posture.Defense));
        }
        [TestMethod]
        public void UnitSetup_UnitTestState_AlwaysMiss()
        {
            Unit unit = new Infantry(true, false);
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
        public static Army CreateWithPlanes(int fighters, int bombers)
        {
            Army attacker = new Army();
            attacker.AddFighters(fighters, true, true);
            attacker.AddBombers(bombers, true, true);
            return attacker;
        }
        [TestMethod]
        public void Army_AATest_AACantHit()
        {
            Army defender = new Army();
            defender.AddAA(1, true, true);
            Army attacker = CreateWithPlanes(1, 0);
            //Defender can't fight if it only has AA
            Assert.IsFalse(defender.CanStillFight());
            Assert.IsTrue(attacker.CanStillFight());
            IOutcome outcome = ArmyOutcome.Fight(attacker, defender);
            Assert.IsFalse(defender.CanStillFight());
            //You lose your AA even if you can't fight
            Assert.IsFalse(defender.HasAA());
            Assert.IsTrue(attacker.CanStillFight());
            
        }
        [TestMethod]
        public void Army_AATest_AAWins2()
        {
            //Defender needs to have a dude, you can't defend with just AA
            Army defender = new Army();
            defender.AddAA(1, true, true);
            defender.AddInfantry(1, true, false);
            Army attacker = CreateWithPlanes(1, 1);

            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(defender.HasAA());
            Assert.IsTrue(attacker.CanStillFight());            
            IOutcome outcome = ArmyOutcome.Fight(attacker, defender);
            Assert.IsTrue(defender.CanStillFight());
            Assert.IsFalse(attacker.CanStillFight());
        }
        [TestMethod]
        public void Army_AATest_AAWins3()
        {
            Army defender = new Army();
            defender.AddInfantry(1, true, false);
            defender.AddAA(1, true, true);
            Army attacker = CreateWithPlanes(1, 2);

            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(attacker.CanStillFight());
            IOutcome outcome = ArmyOutcome.Fight(attacker, defender);
            Assert.IsTrue(defender.CanStillFight());
            Assert.IsFalse(attacker.CanStillFight());
        }
        [TestMethod]
        public void Army_AATest_AALoses()
        {
            Army defender = new Army();
            defender.AddAA(1, true, true);
            defender.AddInfantry(1, true, false);
            Army attacker = CreateWithPlanes(1, 3);

            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(attacker.CanStillFight());
            IOutcome outcome = ArmyOutcome.Fight(attacker, defender);
            Assert.IsFalse(defender.CanStillFight());
            Assert.IsTrue(attacker.CanStillFight());
        }
    }
    [TestClass]
    public class OutcomeUnitTests
    {
        [TestMethod]
        public void Outcome_FightFindWinner_AttackerWins()
        {
            IOutcome outcome = TestHelpers.CreateAttackerWin();

            Assert.IsTrue(outcome.AttackerCanStillFight());
            Assert.IsFalse(outcome.DefenderCanStillFight());
            Assert.AreEqual(outcome.DefenderRemainingUnits(), 0);
            Assert.AreNotEqual(outcome.AttackerRemainingUnits(), 0);
        }
        [TestMethod]
        public void Outcome_FightFindWinner_DefenderWins()
        {
            IOutcome outcome = TestHelpers.CreateDefenderWin();

            Assert.IsFalse(outcome.AttackerCanStillFight());
            Assert.IsTrue(outcome.DefenderCanStillFight());
            Assert.AreNotEqual(outcome.DefenderRemainingUnits(), 0);
            Assert.AreEqual(outcome.AttackerRemainingUnits(), 0);
        }
        [TestMethod]
        public void Outcome_FightFindWinner_Tie()
        {
            IOutcome outcome = TestHelpers.CreateTie();

            Assert.IsFalse(outcome.AttackerCanStillFight());
            Assert.IsFalse(outcome.DefenderCanStillFight());
            Assert.AreEqual(outcome.DefenderRemainingUnits(), 0);
            Assert.AreEqual(outcome.AttackerRemainingUnits(), 0);
        }
        [TestMethod]
        public void Outcome_FightInfantryHitEverything_AttackerWins()
        {
            Army attacker = new Army();
            attacker.AddInfantry(1, true, true);
            Army defender = TestHelpers.CreateWithTestUnits(2, true, true, false);
            Assert.IsTrue(attacker.CanStillFight());
            Assert.IsTrue(defender.CanStillFight());
            IOutcome outcome = ArmyOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Attack, outcome.Winner);
            Assert.IsTrue(outcome.AttackerCanStillFight());
            Assert.IsFalse(outcome.DefenderCanStillFight());
        }
        [TestMethod]
        public void Outcome_FightInfantryHitEverything_DefenderWins()
        {
            Army defender = new Army();
            defender.AddInfantry(1, true, true);
            Army attacker = TestHelpers.CreateWithTestUnits(2, false, true, false);
            Assert.IsTrue(attacker.CanStillFight());
            Assert.IsTrue(defender.CanStillFight());
            IOutcome outcome = ArmyOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Defense, outcome.Winner);
            Assert.IsFalse(outcome.AttackerCanStillFight());
            Assert.IsTrue(outcome.DefenderCanStillFight());
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
