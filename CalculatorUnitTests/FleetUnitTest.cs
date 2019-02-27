using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
using System.Reflection;

namespace CalculatorUnitTests
{
    public class FleetTestHelpers
    {
        public static Fleet CreateWithTestUnits(int count, bool isTest, bool alwaysHit)
        {
            Fleet fleet = new Fleet();
            fleet.AddAircraftCarriers(count, isTest, alwaysHit);
            fleet.AddBattleships(count, isTest, alwaysHit);
            fleet.AddDestroyers(count, isTest, alwaysHit);
            fleet.AddCruisers(count, isTest, alwaysHit);
            fleet.AddSubmarines(count, isTest, alwaysHit);
            fleet.AddBombers(count, isTest, alwaysHit);
            fleet.AddFighters(count, isTest, alwaysHit); 
            return fleet;
        }

    }
    [TestClass]
    public class FleetUnitTest
    {
        [TestMethod]
        public void Fleet_HitBattleshipTwice_Attacker()
        {
            Fleet fleet = new Fleet();
            fleet.AddBattleships(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());
            fleet.RemoveSurfaceHitsAttacker(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());

            fleet.RemoveSurfaceHitsAttacker(1);
            Assert.IsFalse(fleet.CanStillFight());
            Assert.AreEqual(0, fleet.NumberOfRemainingUnits());
        }
        [TestMethod]
        public void Fleet_HitBattleshipTwice_Defender()
        {
            Fleet fleet = new Fleet();
            fleet.AddBattleships(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());
            fleet.RemoveSurfaceHitsDefender(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());

            fleet.RemoveSurfaceHitsDefender(1);
            Assert.IsFalse(fleet.CanStillFight());
            Assert.AreEqual(0, fleet.NumberOfRemainingUnits());
        }
        [TestMethod]
        public void Fleet_HasSurfaceShips_Destroyer()
        {
            Fleet fleet = new Fleet();
            Assert.IsFalse(fleet.HasSurfaceShips());
            Assert.IsFalse(fleet.CanStillFight());
            fleet.AddDestroyers(1);
            Assert.IsTrue(fleet.HasSurfaceShips());
            Assert.IsTrue(fleet.CanStillFight());
        }
        [TestMethod]
        public void Fleet_HasSurfaceShips_AircraftCarrier()
        {
            Fleet fleet = new Fleet();
            Assert.IsFalse(fleet.HasSurfaceShips());
            Assert.IsFalse(fleet.CanStillFight());
            fleet.AddAircraftCarriers(1);
            Assert.IsTrue(fleet.HasSurfaceShips());
            Assert.IsTrue(fleet.CanStillFight());
        }
        [TestMethod]
        public void Fleet_HasSurfaceShips_Cruiser()
        {
            Fleet fleet = new Fleet();
            Assert.IsFalse(fleet.HasSurfaceShips());
            Assert.IsFalse(fleet.CanStillFight());
            fleet.AddCruisers(1);
            Assert.IsTrue(fleet.HasSurfaceShips());
            Assert.IsTrue(fleet.CanStillFight());
        }
        [TestMethod]
        public void Fleet_HasSurfaceShips_Battleship()
        {
            Fleet fleet = new Fleet();
            Assert.IsFalse(fleet.HasSurfaceShips());
            Assert.IsFalse(fleet.CanStillFight());
            fleet.AddBattleships(1);
            Assert.IsTrue(fleet.HasSurfaceShips());
            Assert.IsTrue(fleet.CanStillFight());
        }
        [TestMethod]
        public void Fleet_HasSurfaceShips_Subs()
        {
            Fleet fleet = new Fleet();
            fleet.AddSubmarines(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.IsFalse(fleet.HasSurfaceShips());
        }
        [TestMethod]
        public void Fleet_HasSurfaceShips_SubsPlusSurface()
        {
            Fleet fleet = new Fleet();
            fleet.AddSubmarines(1);
            fleet.AddBattleships(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.IsTrue(fleet.HasSurfaceShips());
        }
        [TestMethod]
        public void Fleet_HasSurfaceShips_PlanesPlusSurface()
        {
            Fleet fleet = new Fleet();
            fleet.AddAircraftCarriers(1);
            fleet.AddBombers(1);
            fleet.AddFighters(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.IsTrue(fleet.HasSurfaceShips());
        }
        [TestMethod]
        public void Fleet_AddingPlanes_TwoFighters()
        {
            Fleet fleet = new Fleet();
            Assert.IsFalse(fleet.CanAddPlanes(1));
            fleet.AddAircraftCarriers(1);
            Assert.IsTrue(fleet.CanAddPlanes(1));
            Assert.IsTrue(fleet.CanAddPlanes(2));
            Assert.IsFalse(fleet.CanAddPlanes(3));
            fleet.AddFighters(1);
            Assert.IsTrue(fleet.CanAddPlanes(1));
            Assert.IsFalse(fleet.CanAddPlanes(2));
            Assert.IsFalse(fleet.CanAddPlanes(3));
            fleet.AddFighters(1);
            Assert.IsFalse(fleet.CanAddPlanes(1));
            Assert.IsFalse(fleet.CanAddPlanes(2));
            Assert.IsFalse(fleet.CanAddPlanes(3));
            Assert.ThrowsException<Exception>(() => fleet.AddFighters(1));
        }
        [TestMethod]
        public void Fleet_AddingPlanes_TwoBombers()
        {
            Fleet fleet = new Fleet();
            Assert.IsFalse(fleet.CanAddPlanes(1));
            fleet.AddAircraftCarriers(1);
            Assert.IsTrue(fleet.CanAddPlanes(1));
            Assert.IsTrue(fleet.CanAddPlanes(2));
            Assert.IsFalse(fleet.CanAddPlanes(3));
            fleet.AddBombers(1);
            Assert.IsTrue(fleet.CanAddPlanes(1));
            Assert.IsFalse(fleet.CanAddPlanes(2));
            Assert.IsFalse(fleet.CanAddPlanes(3));
            fleet.AddBombers(1);
            Assert.IsFalse(fleet.CanAddPlanes(1));
            Assert.IsFalse(fleet.CanAddPlanes(2));
            Assert.IsFalse(fleet.CanAddPlanes(3));
            Assert.ThrowsException<Exception>(() => fleet.AddBombers(1));
        }
        [DataTestMethod]
        [DataRow(typeof(Cruiser), "AddCruisers", 1)]
        [DataRow(typeof(Destroyer), "AddDestroyers", 1)]
        [DataRow(typeof(Battleship), "AddBattleships", 1)]
        [DataRow(typeof(AircraftCarrier), "AddAircraftCarriers", 1)]
        [DataRow(typeof(Submarine), "AddSubmarines", 1)]
        [DataRow(typeof(Cruiser), "AddCruisers", 10)]
        [DataRow(typeof(Destroyer), "AddDestroyers", 10)]
        [DataRow(typeof(Battleship), "AddBattleships", 10)]
        [DataRow(typeof(AircraftCarrier), "AddAircraftCarriers", 10)]
        [DataRow(typeof(Submarine), "AddSubmarines", 10)]
        [DataRow(typeof(Cruiser), "AddCruisers", 0)]
        [DataRow(typeof(Destroyer), "AddDestroyers", 0)]
        [DataRow(typeof(Battleship), "AddBattleships", 0)]
        [DataRow(typeof(AircraftCarrier), "AddAircraftCarriers", 0)]
        [DataRow(typeof(Submarine), "AddSubmarines", 0)]
        public void Fleet_IPCCount_SurfaceShips(Type unitType, string addMethodName, int unitsToAdd)
        {
            Unit unit = null;
            unit = Activator.CreateInstance(unitType, new object[] { false, false }) as Unit;
            Fleet fleet = new Fleet();
            Type t = typeof(Fleet);
            MethodInfo addMethod = t.GetMethod(addMethodName);
            addMethod.Invoke(fleet, new object[] { unitsToAdd, false, false });
            Assert.AreEqual(unitsToAdd * unit.IpcValue, fleet.CurrentIpcValue());
        }
        [DataTestMethod]
        [DataRow(typeof(Fighter), "AddFighters", 1)]
        [DataRow(typeof(Fighter), "AddFighters", 10)]
        [DataRow(typeof(Fighter), "AddFighters", 0)]
        [DataRow(typeof(Bomber), "AddBombers", 1)]
        [DataRow(typeof(Bomber), "AddBombers", 10)]
        [DataRow(typeof(Bomber), "AddBombers", 0)]
        public void Fleet_IPCCount_Planes(Type unitType, string addMethodName, int unitsToAdd)
        {
            //Have to add an Aircraft carrier for every plane that is added
            AircraftCarrier carrier = new AircraftCarrier(true, false);
            Unit unit = null;
            unit = Activator.CreateInstance(unitType, new object[] { false, false }) as Unit;
            Fleet fleet = new Fleet();
            fleet.AddAircraftCarriers(unitsToAdd);
            Type t = typeof(Fleet);
            MethodInfo addMethod = t.GetMethod(addMethodName);
            addMethod.Invoke(fleet, new object[] { unitsToAdd, false, false });
            Assert.AreEqual(unitsToAdd * (unit.IpcValue + carrier.IpcValue), fleet.CurrentIpcValue());
        }

        [TestMethod]
        public void Fleet_CanFightEachOther_SubsOnly()
        {
            Fleet attacker = new Fleet();
            Fleet defender = new Fleet();
            attacker.AddSubmarines(1);
            defender.AddSubmarines(1);
            Assert.IsTrue(attacker.CanStillFight());
            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            Assert.IsTrue(FleetOutcome.CanFightEachOther(defender, attacker));
        }
        [TestMethod]
        public void Fleet_CanFightEachOther_SubVsDestroyer()
        {
            Fleet attacker = new Fleet();
            Fleet defender = new Fleet();
            attacker.AddSubmarines(1);
            defender.AddDestroyers(1);
            Assert.IsTrue(attacker.CanStillFight());
            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            Assert.IsTrue(FleetOutcome.CanFightEachOther(defender, attacker));
        }
        [TestMethod]
        public void Fleet_CanFightEachOther_SubVsBattleship()
        {
            Fleet attacker = new Fleet();
            Fleet defender = new Fleet();
            attacker.AddSubmarines(1);
            defender.AddBattleships(1);
            Assert.IsTrue(attacker.CanStillFight());
            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            Assert.IsTrue(FleetOutcome.CanFightEachOther(defender, attacker));
        }
        [TestMethod]
        public void Fleet_CanFightEachOther_SubVsPlane()
        {
            //Fill this in when there is a way to remove the aircraft carrier
            //without removing the plane. Until then, ignore this test
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void Fleet_CanFightEachOther_FullFleets()
        {
            Fleet attacker = FleetTestHelpers.CreateWithTestUnits(1, false, false);
            Fleet defender = FleetTestHelpers.CreateWithTestUnits(1, false, false);
            Assert.IsTrue(attacker.CanStillFight());
            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            Assert.IsTrue(FleetOutcome.CanFightEachOther(defender, attacker));
        }
        [TestMethod]
        public void Fleet_CanFightEachOther_PlanesVsPlanes()
        {
            Fleet attacker = new Fleet();
            Fleet defender = new Fleet();
            attacker.AddAircraftCarriers(1);
            attacker.AddFighters(2);
            defender.AddAircraftCarriers(1);
            defender.AddFighters(2);
            Assert.IsTrue(attacker.CanStillFight());
            Assert.IsTrue(defender.CanStillFight());
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            Assert.IsTrue(FleetOutcome.CanFightEachOther(defender, attacker));
        }
        [TestMethod]
        public void Fleet_SupriseAttack_SubAlwaysHit()
        {
            Fleet fleet = new Fleet();
            fleet.AddSubmarines(1, true, true);
            Assert.AreEqual(1, fleet.RollSubmarineAttack());
            Assert.AreEqual(1, fleet.RollSubmarineDefense());
        }
        [TestMethod]
        public void Fleet_RemoveSubmarineHits_PlanesAndNoPlanes()
        {
            Fleet fleet = new Fleet();
            fleet.AddAircraftCarriers(1);
            fleet.AddBombers(2);
            Assert.IsTrue(fleet.HasSurfaceShips());
            Assert.AreEqual(2, fleet.NumberOfPlanes());
            fleet.RemoveSubmarineHits(3);
            Assert.IsFalse(fleet.HasSurfaceShips());
            Assert.AreEqual(2, fleet.NumberOfPlanes());
        }
        [TestMethod]
        public void Fleet_AttackersWin_FullFleet()
        {
            Fleet attacker = FleetTestHelpers.CreateWithTestUnits(3, true, true);
            Fleet defender = FleetTestHelpers.CreateWithTestUnits(3, true, false);
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Attack, outcome.Winner);
            Assert.IsTrue(outcome.AttackerCanStillFight);
            Assert.IsFalse(outcome.DefenderCanStillFight);
        }
        [TestMethod]
        public void Fleet_DefendersWin_FullFleet()
        {
            Fleet attacker = FleetTestHelpers.CreateWithTestUnits(3, true, false);
            Fleet defender = FleetTestHelpers.CreateWithTestUnits(3, true, true);
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Defense, outcome.Winner);
            Assert.IsFalse(outcome.AttackerCanStillFight);
            Assert.IsTrue(outcome.DefenderCanStillFight);
        }
        [TestMethod]
        public void Fleet_SubsVsPlanes_Stalemate()
        {
            Fleet attacker = new Fleet();
            Fleet defender = new Fleet();
            attacker.AddAircraftCarriers(1, true, false);
            attacker.AddFighters(2, true, true);
            defender.AddSubmarines(1, true, true);
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Stalemate, outcome.Winner);
            Assert.IsTrue(outcome.AttackerCanStillFight);
            Assert.IsTrue(outcome.DefenderCanStillFight);
            Assert.AreEqual(2, attacker.PlanesWithoutLandingLocation());
        }
        [TestMethod]
        public void Fleet_TieFight_OnlySubs()
        {
            Fleet attacker = new Fleet();
            Fleet defender = new Fleet();
            attacker.AddSubmarines(3, true, true);
            defender.AddSubmarines(3, true, true);
            int intialDefenderIPC = defender.CurrentIpcValue();
            int intialAttackerIPC = attacker.CurrentIpcValue();
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.None, outcome.Winner);
            Assert.IsFalse(outcome.AttackerCanStillFight);
            Assert.IsFalse(outcome.DefenderCanStillFight);
            Assert.AreEqual(intialAttackerIPC, outcome.AttackerIpcLosses);
            Assert.AreEqual(intialDefenderIPC, outcome.DefenderIpcLosses);
        }
        [TestMethod]
        public void Fleet_SurfaceShipsHittingSubs_AttackersWin()
        {
            Fleet attacker = new Fleet();
            attacker.AddBattleships(1, true, true);
            attacker.AddAircraftCarriers(1, true, false);
            attacker.AddBombers(1, true, false);
            Fleet defender = new Fleet();
            defender.AddSubmarines(3, true, false);
            defender.AddBattleships(3, true, false);
            int intialDefenderIPC = defender.CurrentIpcValue();
            int intialAttackerIPC = attacker.CurrentIpcValue();
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Attack, outcome.Winner);
            Assert.IsTrue(outcome.AttackerCanStillFight);
            Assert.IsFalse(outcome.DefenderCanStillFight);
            Assert.AreEqual(intialDefenderIPC, outcome.DefenderIpcLosses + defender.CurrentIpcValue());
            Assert.AreEqual(intialAttackerIPC, outcome.AttackerIpcLosses + attacker.CurrentIpcValue());
        }
        [TestMethod]
        public void Fleet_SurfaceShipsHittingSubs_DefendersWin()
        {
            Fleet attacker = new Fleet();
            attacker.AddSubmarines(3, true, false);
            attacker.AddAircraftCarriers(1, true, false);
            attacker.AddBombers(1, true, false);
            Fleet defender = new Fleet();
            defender.AddBattleships(3, true, true);
            int intialDefenderIPC = defender.CurrentIpcValue();
            int intialAttackerIPC = attacker.CurrentIpcValue();
            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Defense, outcome.Winner);
            Assert.IsFalse(outcome.AttackerCanStillFight);
            Assert.IsTrue(outcome.DefenderCanStillFight);
            Assert.AreEqual(intialDefenderIPC, outcome.DefenderIpcLosses + defender.CurrentIpcValue());
            Assert.AreEqual(intialAttackerIPC, outcome.AttackerIpcLosses + attacker.CurrentIpcValue());
        }
        [TestMethod]
        public void Fleet_PlanesHittingEverything_AttackersWin()
        {
            Fleet attacker = new Fleet();
            attacker.AddAircraftCarriers(1, true, false);
            attacker.AddDestroyers(1, true, false);
            attacker.AddBombers(1, true, true);
            attacker.AddFighters(1, true, true);
            Fleet defender = FleetTestHelpers.CreateWithTestUnits(2, true, false);
            int intialDefenderIPC = defender.CurrentIpcValue();
            int intialAttackerIPC = attacker.CurrentIpcValue();

            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Attack, outcome.Winner);
            Assert.IsTrue(outcome.AttackerCanStillFight);
            Assert.IsFalse(outcome.DefenderCanStillFight);
            Assert.AreEqual(intialDefenderIPC, outcome.DefenderIpcLosses + defender.CurrentIpcValue());
            Assert.AreEqual(intialAttackerIPC, outcome.AttackerIpcLosses + attacker.CurrentIpcValue());
        }
        [TestMethod]
        public void Fleet_PlanesHittingEverything_DefendersWin()
        {
            Fleet defender = new Fleet();
            defender.AddAircraftCarriers(1, true, false);
            defender.AddDestroyers(1, true, false);
            defender.AddBombers(1, true, true);
            defender.AddFighters(1, true, true);
            Fleet attacker = FleetTestHelpers.CreateWithTestUnits(2, true, false);
            int intialDefenderIPC = defender.CurrentIpcValue();
            int intialAttackerIPC = attacker.CurrentIpcValue();

            Assert.IsTrue(FleetOutcome.CanFightEachOther(attacker, defender));
            IOutcome outcome = FleetOutcome.Fight(attacker, defender);
            Assert.AreEqual(Posture.Defense, outcome.Winner);
            Assert.IsFalse(outcome.AttackerCanStillFight);
            Assert.IsTrue(outcome.DefenderCanStillFight);
            Assert.AreEqual(intialDefenderIPC, outcome.DefenderIpcLosses + defender.CurrentIpcValue());
            Assert.AreEqual(intialAttackerIPC, outcome.AttackerIpcLosses + attacker.CurrentIpcValue());
        }
        [TestMethod]
        public void Fleet_MakeEmptyFleet()
        {
            Fleet fleet = FleetTestHelpers.CreateWithTestUnits(0, false, false);
            Assert.IsFalse(fleet.CanStillFight());
        }
    }
}
