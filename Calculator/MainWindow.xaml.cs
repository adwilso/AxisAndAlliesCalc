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
    ///  Minimal number of units to win a battle given an opponent
    /// </summary>
    /// 

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
                var outcome = Outcome.Fight(attackers, defenders);
                results.Outcomes.Add(outcome);
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
            int unitCount= Int32.Parse(attInf.Text);
            attackers.AddInfantry(unitCount);            

            unitCount = Int32.Parse(attSupInf.Text);
            attackers.AddSupportedInfantry(unitCount);            

            unitCount = Int32.Parse(attTank.Text);
            attackers.AddTanks(unitCount);

            unitCount = Int32.Parse(attArt.Text);
            attackers.AddArtillery(unitCount);
           
            unitCount = Int32.Parse(attFight.Text);
            attackers.AddFighters(unitCount);

            unitCount = Int32.Parse(attBomb.Text);
            attackers.AddBombers(unitCount);

            return;
        }

        private void GetDefenders(Army defenders)
        {
            //We are throwing on bad input. Way easier in V1
            Int32 unitCount = Int32.Parse(defInf.Text);
            defenders.AddInfantry(unitCount);            

            unitCount = Int32.Parse(defSupInf.Text);
            defenders.AddSupportedInfantry(unitCount);

            unitCount = Int32.Parse(defTank.Text);
            defenders.AddTanks(unitCount);
            
            unitCount = Int32.Parse(defAA.Text);
            defenders.AddAA(unitCount);


            unitCount = Int32.Parse(defArt.Text);
            defenders.AddTanks(unitCount);

            unitCount = Int32.Parse(defFight.Text);
            defenders.AddFighters(unitCount);

            unitCount = Int32.Parse(defBomb.Text);
            defenders.AddBombers(unitCount);

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
