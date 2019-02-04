using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// To do list: 
    ///  Output the stats on the main form
    ///    Record information such as median losses
    ///    More information on ties
    ///  Enable naval battles
    ///  Change the order that units are eliminated
    ///  Handle the cases where infantry goes from supported to unsupported on attack
    ///  Gracefully handle the cases where the data was malformed
    ///  Reset button to clear all the text boxes
    ///  Save button for common territories
    ///  Minimal number of units to win a battle give a fight
    /// </summary>
    /// 
    public class Posture
    {
        public const int None = 0;
        public const int Defense = 1;
        public const int Attack = 2;

    }

    public class Outcome
    {
        
        public int Winner = Posture.None;
        public Army FinalAttacker;
        public Army FinalDefender;

        private static void debug(string input)
        {
            //Debug.WriteLine(input);
        }

        public override string ToString()
        {
            string retval;
            if (Winner == Posture.Attack)
            {
                retval = "Attacker";
            }
            else if (Winner == Posture.Defense)
            {
                retval = "Defender";
            }
            else if (Winner == Posture.None)
            {
                retval = "Tie";
            }
            else
            {
                throw new Exception ("ToString In Outcome fucked up");
            }
            return retval;
        }

        public static Outcome Fight(Army attackers, Army defenders)
        {
            Outcome outcome = new Outcome();
            //Pre comabat - roll one dice for each attacking plane, up to 3 for each AA
            if (defenders.AA.Count > 0)
            {
                int attackingPlanes = attackers.Fighters.Count + attackers.Bombers.Count;
                if (attackingPlanes > 0)
                {
                    int aaHitCount = 0;
                    foreach (var aa in defenders.AA)
                    {
                        //3 rolls or one for each plane
                        for (int i = 0; i < attackingPlanes && i < 3; i++)
                        {
                            if (aa.doesHit(Posture.Defense))
                            {
                                debug("AA Hit");
                                aaHitCount += 1;
                            }
                        }
                    }
                    attackers.RemoveAirForce(aaHitCount);

                }
            }

            while (attackers.CanStillFight() && defenders.CanStillFight())
            {
                //Combat - round N. Fight!
                debug("Attacking Army");
                debug(attackers.ToString());
                debug("Defending Army");
                debug(defenders.ToString());
                debug("Defender Rolls");
                int defenderHitCount = defenders.RollDefence();
                debug("Total: " + defenderHitCount);
                debug("Attacker Rolls");
                int attackerHitCount = attackers.RollOffense();
                debug("Total: " + attackerHitCount);

                defenders.RemoveGroundForceDefender(attackerHitCount);
                attackers.RemoveGroundForceAttacker(defenderHitCount);
            }
            debug("Combat is over");
            if (!defenders.CanStillFight() && attackers.CanStillFight())
            {
                debug("Attackers Win");
                outcome.Winner = Posture.Attack;
            }
            else if (defenders.CanStillFight() && !attackers.CanStillFight())
            {
                debug("Defenders Win");
                outcome.Winner = Posture.Defense;
            }
            else if (!defenders.CanStillFight() && !attackers.CanStillFight())
            {
                debug("Tie");
                outcome.Winner = Posture.None;
            }
            else
            {
                throw new Exception("We fucked up");
            }
            outcome.FinalAttacker = attackers;
            outcome.FinalDefender = defenders;
            return outcome;
        }


    }

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
                return outcomes; }
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

     public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResetUI();
        }

        public void debug(string text)
        {
           //Debug.WriteLine(text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //You always have to kill infantry before artillery. 
            //That means I don't have to detect if infantry become unsupported
            //midway through an attack
            Army attackers = new Army();
            Army defenders = new Army();
            int rounds = GetRounds();            

            ResultsTracker results = new ResultsTracker();
            for (int i = 0; i < rounds; i++)
            {
                attackers = new Army();
                defenders = new Army();
                GetAttackers(attackers);
                GetDefenders(defenders);
                results.Outcomes.Add(Outcome.Fight(attackers, defenders));
            }

            OutputStats(results);
        }

        private void OutputStats(ResultsTracker results)
        {
            lblAttackerWinRate.Content = results.AttackerWinRate + "%";
            lblDefenderWinRate.Content = results.DefenderWinRate + "%";
            lblTieRate.Content = results.TieRate + "%";
            lblAttackerAverageIPCLost.Content = results.AverageAttackerIPCLost();
            lblDefenderAverageIPCLost.Content = results.AverageDefenderIPCLost();

            Debug.WriteLine("Defender Wins: " + results.DefenderWins + " Percentage: " + results.DefenderWins / results.TotalFights);
            Debug.WriteLine("Defender Average Losses: " + results.DefenderIPCLost / results.TotalFights);
            Debug.WriteLine("Attacker Wins: " + results.AttackerWins + " Percentage: " + results.AttackerWins / results.TotalFights);
            Debug.WriteLine("Attacker Average Losses: " + results.AttackerIPCLost / results.TotalFights);

            Debug.WriteLine("========================================================");
        }


        private int GetRounds()
        {
            return Int32.Parse(rounds.Text);            
        }

        private void GetAttackers(Army attackers)
        {
            //We are throwing on bad input. Way easier in V1
            UInt32 unitCount= UInt32.Parse(attInf.Text);
            for (int i = 0; i < unitCount; i++)
            {
                attackers.Infantry.Add(new Infantry());
            }

            unitCount = UInt32.Parse(attSupInf.Text);
            for (int i = 0; i < unitCount; i++)
            {
                attackers.SupportedInfantry.Add(new SupportedInfantry());
            }

            unitCount = UInt32.Parse(attTank.Text);
            for (int i = 0; i < unitCount; i++)
            {
                attackers.Tanks.Add(new Tank());
            }

            unitCount = UInt32.Parse(attArt.Text);
            for (int i = 0; i < unitCount; i++)
            {
                attackers.Artillery.Add(new Artillery());
            }

            unitCount = UInt32.Parse(attAA.Text);
            for (int i = 0; i < unitCount; i++)
            {
                attackers.AA.Add(new AA());
            }

            unitCount = UInt32.Parse(attFight.Text);
            for (int i = 0; i < unitCount; i++)
            {
                attackers.Fighters.Add(new Fighter());
            }

            unitCount = UInt32.Parse(attBomb.Text);
            for (int i = 0; i < unitCount; i++)
            {
                attackers.Bombers.Add(new Bomber());
            }

            return;
        }

        private void GetDefenders(Army defenders)
        {
            //We are throwing on bad input. Way easier in V1
            UInt32 unitCount = UInt32.Parse(defInf.Text);
            for (int i = 0; i < unitCount; i++)
            {
                defenders.Infantry.Add(new Infantry());
            }

            unitCount = UInt32.Parse(defSupInf.Text);
            for (int i = 0; i < unitCount; i++)
            {
                defenders.SupportedInfantry.Add(new SupportedInfantry());
            }

            unitCount = UInt32.Parse(defTank.Text);
            for (int i = 0; i < unitCount; i++)
            {
                defenders.Tanks.Add(new Tank());
            }

            unitCount = UInt32.Parse(defAA.Text);
            for (int i = 0; i < unitCount; i++)
            {
                defenders.AA.Add(new AA());
            }

            unitCount = UInt32.Parse(defArt.Text);
            for (int i = 0; i < unitCount; i++)
            {
                defenders.Artillery.Add(new Artillery());
            }

            unitCount = UInt32.Parse(defFight.Text);
            for (int i = 0; i < unitCount; i++)
            {
                defenders.Fighters.Add(new Fighter());
            }

            unitCount = UInt32.Parse(defBomb.Text);
            for (int i = 0; i < unitCount; i++)
            {
                defenders.Bombers.Add(new Bomber());
            }

            return;
        }

        private void ResetUI()
        {
            defInf.Text = "0";
            defSupInf.Text = "0";
            defArt.Text = "0";
            defAA.Text = "0";
            defTank.Text = "0";
            defFight.Text = "0";
            defBomb.Text = "0";

            attInf.Text = "0";
            attSupInf.Text = "0";
            attArt.Text = "0";
            attAA.Text = "0";
            attTank.Text = "0";
            attFight.Text = "0";
            attBomb.Text = "0";

            rounds.Text = "10000";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

///     <summary>
///     When this is clicked, reset the UI back to the starting positions
///     </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ResetUI();
        }
    }
}
