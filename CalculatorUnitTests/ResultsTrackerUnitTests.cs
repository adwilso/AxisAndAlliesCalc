using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
using System.Reflection;

namespace CalculatorUnitTests
{
    /// <summary>
    /// Summary description for ResultsTrackerUnitTests
    /// </summary>
    [TestClass]
    public class ResultsTrackerUnitTests
    {
        public ResultsTrackerUnitTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        [TestMethod]
        public void ResultsTracker_AttackerWinsAll()
        {
            ResultsTracker results = new ResultsTracker();
            results.AddOutcome(TestHelpers.CreateAttackerWin());
            results.AddOutcome(TestHelpers.CreateAttackerWin());
            results.AddOutcome(TestHelpers.CreateAttackerWin());

            Assert.AreEqual(results.AttackerWins, 3);
            Assert.AreEqual(results.TotalAttackerIPCLost, 0);
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
            results.AddOutcome(TestHelpers.CreateDefenderWin());
            results.AddOutcome(TestHelpers.CreateDefenderWin());
            results.AddOutcome(TestHelpers.CreateDefenderWin());

            Assert.AreEqual(results.AttackerWins, 0);
            Assert.AreEqual(results.TotalDefenderIPCLost, 0);
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
            results.AddOutcome(TestHelpers.CreateTie());
            results.AddOutcome(TestHelpers.CreateTie());
            results.AddOutcome(TestHelpers.CreateTie());

            Assert.AreEqual(results.AttackerWins, 0);
            Assert.AreEqual(results.AttackerWinRate, 0.0);
            Assert.AreEqual(results.DefenderWinRate, 0.0);
            Assert.AreEqual(results.DefenderWins, 0);
            Assert.AreEqual(results.TieRate, 100);
            Assert.AreEqual(results.Ties, 3);
            Assert.AreEqual(results.TotalFights, 3);
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(1000)]
        public void ResultsTracker_AttackerIPCLost(int expectedNumberOfLosses)
        {
            ResultsTracker tracker = new ResultsTracker();
            int numberOfLosses = expectedNumberOfLosses;
            for (int i = 0; i < 100; i++)
            {
                OutcomeMock outcome = TestHelpers.CreateOutcomeWithBogusData();
                //Oh this is horrifying, but if it works....
                numberOfLosses = TestHelpers.SetPropertyAndReturnRemainder(
                    (n) => outcome.AttackerIpcLosses = n, numberOfLosses);
                tracker.AddOutcome(outcome);
            }
            Assert.AreEqual(expectedNumberOfLosses, tracker.TotalAttackerIPCLost + numberOfLosses);
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(1000)]
        public void ResultsTracker_DefenderIPCLost(int expectedNumberOfLosses)
        {
            ResultsTracker tracker = new ResultsTracker();
            int numberOfLosses = expectedNumberOfLosses;
            for (int i = 0; i < 100; i++)
            {
                OutcomeMock outcome = TestHelpers.CreateOutcomeWithBogusData();
                //Oh this is horrifying, but if it works....
                numberOfLosses = TestHelpers.SetPropertyAndReturnRemainder(
                    (n) => outcome.DefenderIpcLosses = n, numberOfLosses);
                tracker.AddOutcome(outcome);
            }
            Assert.AreEqual(expectedNumberOfLosses, tracker.TotalDefenderIPCLost + numberOfLosses);
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10000)]
        [DataRow(125)]
        public void ResultsTracker_GetNumberOfWins_AttackerWins(int expectedNumberOfWins)
        {
            ResultsTracker tracker = new ResultsTracker();
            for (int i = 0; i < expectedNumberOfWins; i++)
            {
                tracker.AddOutcome(TestHelpers.CreateAttackerWin());
            }
            Assert.AreEqual(expectedNumberOfWins, tracker.AttackerWins);
            Assert.AreEqual(0, tracker.DefenderWins);
            Assert.AreEqual(0, tracker.Ties);
            Assert.AreEqual(100, tracker.AttackerWinRate);
            Assert.AreEqual(0, tracker.DefenderWinRate);
            Assert.AreEqual(0, tracker.TieRate);
            Assert.AreEqual(0, tracker.Stalemates);
            Assert.AreEqual(0, tracker.StalemateRate);
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10000)]
        [DataRow(125)]
        public void ResultsTracker_GetNumberOfWins_DefenderWins(int expectedNumberOfWins)
        {
            ResultsTracker tracker = new ResultsTracker();
            for (int i = 0; i < expectedNumberOfWins; i++)
            {
                tracker.AddOutcome(TestHelpers.CreateDefenderWin());
            }
            Assert.AreEqual(expectedNumberOfWins, tracker.DefenderWins);
            Assert.AreEqual(0, tracker.AttackerWins);
            Assert.AreEqual(0, tracker.Ties);
            Assert.AreEqual(0, tracker.AttackerWinRate);
            Assert.AreEqual(100, tracker.DefenderWinRate);
            Assert.AreEqual(0, tracker.TieRate);
            Assert.AreEqual(0, tracker.Stalemates);
            Assert.AreEqual(0, tracker.StalemateRate);
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10000)]
        [DataRow(125)]
        public void ResultsTracker_GetNumberOfTies_AllTies(int expectedNumberOfTies)
        {
            ResultsTracker tracker = new ResultsTracker();
            for (int i = 0; i < expectedNumberOfTies; i++)
            {
                tracker.Outcomes.Add(TestHelpers.CreateTie());
            }
            Assert.AreEqual(0, tracker.DefenderWins);
            Assert.AreEqual(0, tracker.AttackerWins);
            Assert.AreEqual(expectedNumberOfTies, tracker.Ties);
            Assert.AreEqual(0, tracker.AttackerWinRate);
            Assert.AreEqual(0, tracker.DefenderWinRate);
            Assert.AreEqual(100, tracker.TieRate);
            Assert.AreEqual(0, tracker.Stalemates);
            Assert.AreEqual(0, tracker.StalemateRate);
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10000)]
        [DataRow(125)]
        public void ResultsTracker_GetNumberOfTies_AllStalemate(int expectedStalemates)
        {
            ResultsTracker tracker = new ResultsTracker();
            for (int i = 0; i < expectedStalemates; i++)
            {
                tracker.Outcomes.Add(TestHelpers.CreateStalemate());
            }
            Assert.AreEqual(0, tracker.DefenderWins);
            Assert.AreEqual(0, tracker.AttackerWins);
            Assert.AreEqual(0, tracker.Ties);
            Assert.AreEqual(0, tracker.AttackerWinRate);
            Assert.AreEqual(0, tracker.DefenderWinRate);
            Assert.AreEqual(0, tracker.TieRate);
            Assert.AreEqual(expectedStalemates, tracker.Stalemates);
            Assert.AreEqual(100, tracker.StalemateRate);
        }
    }
}
