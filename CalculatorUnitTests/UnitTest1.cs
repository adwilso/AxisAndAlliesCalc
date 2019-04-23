using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
using System.Reflection;

namespace CalculatorUnitTests
{
    public class TestHelpers
    {
        public static Random rnd = new Random();        
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
        public static IOutcome CreateStalemate()
        {
            Fleet attacker = new Fleet();
            attacker.AddAircraftCarriers(1, true, false);
            attacker.AddBombers(2, true, true);
            Fleet defender = new Fleet();
            defender.AddSubmarines(1, true, true);
            return FleetOutcome.Fight(attacker, defender);
        }
        public static int SetPropertyAndReturnRemainder(Action<int> t, int maxValue)
        {
            int value = rnd.Next(0, maxValue);
            t.Invoke(value);
            return maxValue - value;
        }
        public static OutcomeMock CreateOutcomeWithBogusData()
        {
            OutcomeMock outcome = new OutcomeMock();
            outcome.AttackerIpcLosses = 0;
            outcome.DefenderIpcLosses = 0;
            outcome.DefenderIpcRemaining = 0;
            outcome.AttackerIpcRemaining = 0;
            outcome.AttackerNumberOfUnits = 0;
            outcome.DefenderNumberOfUnits = 0;
            outcome.Winner = Posture.None;
            outcome.AttackerCanStillFight = false;
            outcome.DefenderCanStillFight = false;
            outcome.AttackerRemainingUnits = 0;
            outcome.DefenderRemainingUnits = 0;
            return outcome; 
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
        [DataTestMethod]
        [DataRow(typeof(AA), "AddAA", 1)]
        [DataRow(typeof(Infantry), "AddInfantry", 1)]
        [DataRow(typeof(SupportedInfantry), "AddSupportedInfantry", 1)]
        [DataRow(typeof(Artillery), "AddArtillery", 1)]
        [DataRow(typeof(Tank), "AddTanks", 1)]
        [DataRow(typeof(Fighter), "AddFighters", 1)]
        [DataRow(typeof(Bomber), "AddBombers", 1)]
        [DataRow(typeof(AA), "AddAA", 10)]
        [DataRow(typeof(Infantry), "AddInfantry", 10)]
        [DataRow(typeof(SupportedInfantry), "AddSupportedInfantry", 10)]
        [DataRow(typeof(Artillery), "AddArtillery", 10)]
        [DataRow(typeof(Tank), "AddTanks", 10)]
        [DataRow(typeof(Fighter), "AddFighters", 10)]
        [DataRow(typeof(Bomber), "AddBombers", 10)]
        [DataRow(typeof(AA), "AddAA", 0)]
        [DataRow(typeof(Infantry), "AddInfantry", 0)]
        [DataRow(typeof(SupportedInfantry), "AddSupportedInfantry", 0)]
        [DataRow(typeof(Artillery), "AddArtillery", 0)]
        [DataRow(typeof(Tank), "AddTanks", 0)]
        [DataRow(typeof(Fighter), "AddFighters", 0)]
        [DataRow(typeof(Bomber), "AddBombers", 0)]
        public void Army_IPCCountTest(Type unitType, string addMethodName, int unitsToAdd)
        {
            Unit unit = null;
            unit = Activator.CreateInstance(unitType, new object[] { false, false }) as Unit;
            Army army = new Army();
            Type t = typeof(Army);
            MethodInfo addMethod = t.GetMethod(addMethodName);
            addMethod.Invoke(army, new object[] { unitsToAdd, false, false });
            Assert.AreEqual(unitsToAdd * unit.IpcValue, army.CurrentIpcValue());
        }
        //Need to move supported to unsupported and vice versa
        //Also need to test that when a battle happens, you can remove
        //inf or art and have things shuffle correctly
        [DataTestMethod]
        [DataRow(1,1)] //Move it over
        [DataRow(100, 100)] //Volume test
        [DataRow(10, 5)] //Not all infantry
        [DataRow(5, 10)] //too much artillery
        [DataRow(10, 0)] //No artillery
        [DataRow(0, 10)] //No infantry
        public void Army_RebalanceInfantry_AddArtillery(int totalInfantry, int totalArtillery)
        {
            //First add infantry to the army,
            Army army = new Army();
            army.AddInfantry(totalInfantry);
            army.Test_ReturnInfantryAndArtilleryCount(out int infCount, out int supCount, out int artCount);
            Assert.AreEqual(infCount, totalInfantry);
            Assert.AreEqual(supCount, 0);
            Assert.AreEqual(artCount, 0);

            army.AddArtillery(totalArtillery);
            army.Test_ReturnInfantryAndArtilleryCount(out infCount, out supCount, out artCount);
            Assert.AreEqual(infCount, Math.Max( 0, totalInfantry - totalArtillery));
            Assert.AreEqual(supCount, Math.Min(totalInfantry, totalArtillery));
            Assert.AreEqual(artCount, totalArtillery);
        }
        [DataTestMethod]
        [DataRow(1, 1)] //Move it over
        [DataRow(100, 100)] //Volume test
        [DataRow(10, 5)] //Not all infantry
        [DataRow(5, 10)] //too much artillery
        [DataRow(10, 0)] //No artillery
        [DataRow(0, 10)] //No infantry
        public void Army_RebalanceInfantry_AddInfantry(int totalInfantry, int totalArtillery)
        {
            //First add artillery to the army
            Army army = new Army();
            army.AddArtillery(totalArtillery);
            army.Test_ReturnInfantryAndArtilleryCount(out int infCount, out int supCount, out int artCount);
            Assert.AreEqual(infCount, 0);
            Assert.AreEqual(supCount, 0);
            Assert.AreEqual(artCount, totalArtillery);

            army.AddInfantry(totalInfantry);
            army.Test_ReturnInfantryAndArtilleryCount(out infCount, out supCount, out artCount);
            Assert.AreEqual(infCount, Math.Max(0, totalInfantry - totalArtillery));
            Assert.AreEqual(supCount, Math.Min(totalInfantry, totalArtillery));
            Assert.AreEqual(artCount, totalArtillery);
        }
        [DataTestMethod]
        [DataRow(1,1,1)] //It works
        [DataRow(100, 100, 100)] //Stress Test
        [DataRow(10, 5, 3)] //Not all infantry supported
        [DataRow(5, 10, 10)] //Too much artillery
        [DataRow(0, 10, 4)] //No infantry
        [DataRow(10, 0, 4)] //No artillery
        [DataRow(10, 10, 100)] //Total loss
        public void Army_RebalanceInfatry_RemoveUnits(int totalInfantry, int totalArtillery, int unitsToRemove)
        {
            Army army = new Army();
            army.AddArtillery(totalArtillery);
            army.AddInfantry(totalInfantry);

            //Check that we are in a good state here
            army.Test_ReturnInfantryAndArtilleryCount(out int infCount, out int supCount, out int artCount);
            Assert.AreEqual(infCount, Math.Max(0, totalInfantry - totalArtillery));
            Assert.AreEqual(supCount, Math.Min(totalInfantry, totalArtillery));
            Assert.AreEqual(artCount, totalArtillery);

            //Don't know the order of unit removal, so just test to make sure that we've got the right ratios
            army.RemoveGroundForceAttacker(unitsToRemove);
            army.Test_ReturnInfantryAndArtilleryCount(out infCount, out supCount, out artCount);
            Assert.IsTrue(supCount <= artCount);
            Assert.IsTrue(supCount + infCount <= totalInfantry);
            Assert.IsTrue(artCount <= totalArtillery);
        }
    }
    [TestClass]
    public class OutcomeUnitTests
    {
        [TestMethod]
        public void Outcome_FightFindWinner_AttackerWins()
        {
            IOutcome outcome = TestHelpers.CreateAttackerWin();

            Assert.IsTrue(outcome.AttackerCanStillFight);
            Assert.IsFalse(outcome.DefenderCanStillFight);
            Assert.AreEqual(outcome.DefenderRemainingUnits, 0);
            Assert.AreNotEqual(outcome.AttackerRemainingUnits, 0);
        }
        [TestMethod]
        public void Outcome_FightFindWinner_DefenderWins()
        {
            IOutcome outcome = TestHelpers.CreateDefenderWin();

            Assert.IsFalse(outcome.AttackerCanStillFight);
            Assert.IsTrue(outcome.DefenderCanStillFight);
            Assert.AreNotEqual(outcome.DefenderRemainingUnits, 0);
            Assert.AreEqual(outcome.AttackerRemainingUnits, 0);
        }
        [TestMethod]
        public void Outcome_FightFindWinner_Tie()
        {
            IOutcome outcome = TestHelpers.CreateTie();

            Assert.IsFalse(outcome.AttackerCanStillFight);
            Assert.IsFalse(outcome.DefenderCanStillFight);
            Assert.AreEqual(outcome.DefenderRemainingUnits, 0);
            Assert.AreEqual(outcome.AttackerRemainingUnits, 0);
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
            Assert.IsTrue(outcome.AttackerCanStillFight);
            Assert.IsFalse(outcome.DefenderCanStillFight);
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
            Assert.IsFalse(outcome.AttackerCanStillFight);
            Assert.IsTrue(outcome.DefenderCanStillFight);
        }
    }
}
