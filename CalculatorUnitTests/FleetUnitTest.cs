using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
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
            fleet.RemoveNavalForceAttacker(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());

            fleet.RemoveNavalForceAttacker(1);
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
            fleet.RemoveNavalForceDefender(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());

            fleet.RemoveNavalForceDefender(1);
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
            Assert.ThrowsException<Exception>( () => fleet.AddFighters(1));            
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
    }
}
