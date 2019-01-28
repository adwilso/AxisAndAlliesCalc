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

    }

     public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void debug(string text)
        {
           //Debug.WriteLine(text);
        }

        private Outcome fight(Army attackers, Army defenders)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //You always have to kill infantry before artillery. 
            //That means I don't have to detect if infantry become unsupported
            //midway through an attack
            Army attackers = new Army();
            Army defenders = new Army();
            int rounds = GetRounds();

            

            List<Outcome> outcomes = new List<Outcome>();
            for (int i = 0; i < rounds; i++)
            {
                attackers = new Army();
                defenders = new Army();
                GetAttackers(attackers);
                GetDefenders(defenders);

                outcomes.Add(fight(attackers, defenders));
            }

            int totalFights = 0;
            double attackerLosses = 0;
            double defenderLosses = 0;
            double defenderWins = 0;
            double attackerWins = 0;
            double ties = 0;
            foreach (var outcome in outcomes)
            {
                totalFights += 1; 
                defenderLosses += outcome.FinalDefender.Losses;
                attackerLosses += outcome.FinalAttacker.Losses;
                if (outcome.Winner == Posture.Attack)
                {
                    attackerWins += 1;
                }
                else if (outcome.Winner == Posture.Defense)
                {
                    defenderWins += 1; 
                }
                else if (outcome.Winner == Posture.None)
                {
                    ties += 1;
                }
            }

            Debug.WriteLine("Defender Wins: " + defenderWins + " Percentage: " + defenderWins / totalFights);
            Debug.WriteLine("Defender Average Losses: " + defenderLosses / totalFights);
            Debug.WriteLine("Attacker Wins: " + attackerWins + " Percentage: " + attackerWins / totalFights);
            Debug.WriteLine("Attacker Average Losses: " + attackerLosses / totalFights);

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
