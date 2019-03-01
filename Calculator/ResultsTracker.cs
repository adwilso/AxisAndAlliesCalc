using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{

    public class ResultsTracker
    {
        private bool dirtyCache = true;
        private List<IOutcome> outcomes;
        private int totalFights;
        private double attackerIPCLost;
        private double defenderIPCLost;
        private double attackerWins;
        private double defenderWins;
        private double ties;
        public List<IOutcome> Outcomes
        {
            get
            {
                dirtyCache = true;
                return outcomes;
            }
            set
            {
                dirtyCache = true;
                outcomes = value;
            }
        }
        public int TotalFights
        {
            get
            {
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return totalFights;
            }
            private set
            {
                totalFights = value;
            }
        }
        public double TotalAttackerIPCLost
        {
            get
            {
                return outcomes.Sum(x => x.AttackerIpcLosses);                
            }
            private set
            {
                attackerIPCLost = value;
            }
        }
        public double TotalDefenderIPCLost
        {
            get
            {
                return outcomes.Sum(x => x.DefenderIpcLosses);                
            }
            private set
            {
                defenderIPCLost = value;
            }
        }
        public double DefenderWins
        {
            get
            {
                return outcomes.Count(x => x.Winner == Posture.Defense);
            }
            private set
            {
                defenderWins = value;
            }
        }
        public double AttackerWins
        {
            get
            {
                return outcomes.Count(x => x.Winner == Posture.Attack);                
            }
            private set
            {
                attackerWins = value;
            }
        }
        public double Ties
        {
            get
            {
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return ties;
            }
            private set
            {
                ties = value;
            }
        }
        public double AttackerWinRate
        {
            get
            {
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return Math.Round(AttackerWins / TotalFights * 100, 2);
            }
        }
        public double DefenderWinRate
        {
            get
            {
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return Math.Round(DefenderWins / TotalFights * 100, 2);
            }
        }
        public double TieRate
        {
            get
            {
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return Math.Round(Ties / TotalFights * 100, 2);
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
        private void ComputeResults()
        {
            TotalFights = 0;
            TotalAttackerIPCLost = 0;
            TotalDefenderIPCLost = 0;
            DefenderWins = 0;
            AttackerWins = 0;
            Ties = 0;
            dirtyCache = false;
            //We are hitting the member instead of the property here to avoid dirtying the
            //cache again 
            foreach (var outcome in outcomes)
            {
                TotalFights += 1;

                if (outcome.Winner == Posture.Attack)
                {
                    AttackerWins += 1;
                }
                else if (outcome.Winner == Posture.Defense)
                {
                    DefenderWins += 1;
                }
                else if (outcome.Winner == Posture.None)
                {
                    Ties += 1;
                }
            }
        }
    }

}
