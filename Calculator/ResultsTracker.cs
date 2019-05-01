using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace Calculator
{
    public class ResultsTracker
    {
        private List<IOutcome> Outcomes { get; set; }
        public void AddOutcome(IOutcome outcome)
        {
            Outcomes.Add(outcome);
        }
        public int TotalFights
        {
            get
            {
                return Outcomes.Count;
            }
        }
        public double TotalAttackerIPCLost
        {
            get
            {
                return Outcomes.Sum(x => x.AttackerIpcLosses);
            }
        }
        public double TotalDefenderIPCLost
        {
            get
            {
                return Outcomes.Sum(x => x.DefenderIpcLosses);
            }
        }
        public double DefenderWins
        {
            get
            {
                return Outcomes.Count(x => x.Winner == Posture.Defense);
            }
        }
        public double AttackerWins
        {
            get
            {
                return Outcomes.Count(x => x.Winner == Posture.Attack);
            }
        }
        public double Ties
        {
            get
            {
                return Outcomes.Count(x => x.Winner == Posture.None);
            }
        }
        public double Stalemates
        {
            get
            {
                return Outcomes.Count(x => x.Winner == Posture.Stalemate);
            }
        }
        public double AttackerWinRate
        {
            get
            {
                return Math.Round(AttackerWins / TotalFights * 100, 2);
            }
        }
        public double DefenderWinRate
        {
            get
            {
                return Math.Round(DefenderWins / TotalFights * 100, 2);
            }
        }
        public double TieRate
        {
            get
            {
                return Math.Round(Ties / TotalFights * 100, 2);
            }
        }
        public double StalemateRate
        {
            get
            {
                return Math.Round(Stalemates / TotalFights * 100, 2);
            }
        }            
        public ResultsTracker()
        {
            Outcomes = new List<IOutcome>();
        }
        public double AverageAttackerIPCLost()
        {
            return TotalAttackerIPCLost / TotalFights;
        }
        public double AverageDefenderIPCLost()
        {
            return TotalDefenderIPCLost / TotalFights;
        }
        public double MedianAttackerIPCLost
        {
            get
            {
                return Statistics.Median(Outcomes.Select(x => x.AttackerIpcLosses));
            }
        }
        public double MedianDefenderIPCLost
        {
            get
            {
                return Statistics.Median(Outcomes.Select(x => x.DefenderIpcLosses));
            }
        }
        public Histogram AttackerLossesHistogram()
        {
            var t = Outcomes.Select(x => x.AttackerIpcLosses).ToArray();
            var h = new Histogram(t,100);
            return h;
        }
    }
}
