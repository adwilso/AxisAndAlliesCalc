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
            for (int i = 0; i < count; i++)
            {
                Unit temp = new AircraftCarrier();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                fleet.AircraftCarriers.Add(temp);

                temp = new Battleship();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                fleet.AircraftCarriers.Add(temp);

                temp = new Destroyer();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                fleet.AircraftCarriers.Add(temp);

                temp = new Cruiser();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                fleet.AircraftCarriers.Add(temp);

                temp = new Submarine();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                fleet.AircraftCarriers.Add(temp);

                temp = new Fighter();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                fleet.AircraftCarriers.Add(temp);

                temp = new Bomber();
                temp.IsTest = isTest;
                temp.AlwaysHit = alwaysHit;
                fleet.AircraftCarriers.Add(temp);
            }
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
            fleet.Battleships.Add(new Battleship());
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
            fleet.Battleships.Add(new Battleship());
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());
            fleet.RemoveNavalForceDefender(1);
            Assert.IsTrue(fleet.CanStillFight());
            Assert.AreEqual(1, fleet.NumberOfRemainingUnits());

            fleet.RemoveNavalForceDefender(1);
            Assert.IsFalse(fleet.CanStillFight());
            Assert.AreEqual(0, fleet.NumberOfRemainingUnits());
        }
    }
}
