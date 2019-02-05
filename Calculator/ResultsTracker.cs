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
        private List<Outcome> outcomes;
        private int totalFights;
        private double attackerIPCLost;
        private double defenderIPCLost;
        private double attackerWins;
        private double defenderWins;
        private double ties;
        public List<Outcome> Outcomes
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
        public double AttackerIPCLost
        {
            get
            {
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return attackerIPCLost;
            }
            private set
            {
                attackerIPCLost = value;
            }
        }
        public double DefenderIPCLost
        {
            get
            {
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return defenderIPCLost;
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
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return defenderWins;
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
                if (dirtyCache)
                {
                    ComputeResults();
                }
                return attackerWins;
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
            Outcomes = new List<Outcome>();
        }
        public double AverageAttackerIPCLost()
        {
            return AttackerIPCLost / TotalFights;
        }
        public double AverageDefenderIPCLost()
        {
            return DefenderIPCLost / TotalFights;
        }
        private void ComputeResults()
        {
            TotalFights = 0;
            AttackerIPCLost = 0;
            DefenderIPCLost = 0;
            DefenderWins = 0;
            AttackerWins = 0;
            Ties = 0;
            dirtyCache = false;
            //We are hitting the member instead of the property here to avoid dirtying the
            //cache again 
            foreach (var outcome in outcomes)
            {
                TotalFights += 1;
                DefenderIPCLost += outcome.FinalDefender.Losses;
                AttackerIPCLost += outcome.FinalAttacker.Losses;
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
