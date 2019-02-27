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
            results.Outcomes.Add(TestHelpers.CreateAttackerWin());
            results.Outcomes.Add(TestHelpers.CreateAttackerWin());
            results.Outcomes.Add(TestHelpers.CreateAttackerWin());

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
            results.Outcomes.Add(TestHelpers.CreateDefenderWin());
            results.Outcomes.Add(TestHelpers.CreateDefenderWin());
            results.Outcomes.Add(TestHelpers.CreateDefenderWin());

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
                tracker.Outcomes.Add(outcome);
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
                tracker.Outcomes.Add(outcome);
            }
            Assert.AreEqual(expectedNumberOfLosses, tracker.TotalDefenderIPCLost + numberOfLosses);
        }
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10000)]
        [DataRow(125)]
        public void ResultsTracker_GetNumberOfWins_AttackerWins(int expectedNumberOfWins)
        {
            ResultsTracker tracker = new ResultsTracker();
            for (int i = 0; i < expectedNumberOfWins; i++)
            {
                tracker.Outcomes.Add(TestHelpers.CreateAttackerWin());
            }
            Assert.AreEqual(expectedNumberOfWins, tracker.AttackerWins);
            Assert.AreEqual(0, tracker.DefenderWins);
            Assert.AreEqual(0, tracker.Ties);
        }
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(100)]
        [DataRow(125)]
        public void ResultsTracker_GetNumberOfWins_DefenderWins(int expectedNumberOfWins)
        {
            ResultsTracker tracker = new ResultsTracker();
            for (int i = 0; i < expectedNumberOfWins; i++)
            {
                tracker.Outcomes.Add(TestHelpers.CreateDefenderWin());
            }
            Assert.AreEqual(expectedNumberOfWins, tracker.DefenderWins);
            Assert.AreEqual(0, tracker.AttackerWins);
            Assert.AreEqual(0, tracker.Ties);
        }
    }
}
