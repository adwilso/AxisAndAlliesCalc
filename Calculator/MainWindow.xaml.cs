using MathNet.Numerics.Statistics;
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
using LiveCharts.Configurations;
using LiveCharts;


namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// To do list: 
    ///     
    ///  Output the stats on the main form
    ///    Record information such as median losses
    ///    More information on ties
    ///  Change the order that units are eliminated
     ///  Gracefully handle the cases where the data was malformed
    ///  Reset button to clear all the text boxes
    ///  Save button for common territories
    ///  Minimal number of units to win a battle given an opponent
    ///  Option to flip between attacking and defending
    /// </summary>
    /// 

     public partial class MainWindow : Window
    {
        SimulatorController controller;
        public MainWindow()
        {
            InitializeComponent();
            ResetUI();
            controller = new SimulatorController(this);
        }

        public void debug(string text)
        {
           //Debug.WriteLine(text);
        }
        private void ProcessingFight()
        {
            NavyFight.IsEnabled = false;
        }
        private void FinishedProcessing()
        {
            NavyFight.IsEnabled = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProcessingFight();
            controller.ArmyFight();
            FinishedProcessing();
        }
        public void OutputArmyStats(ResultsTracker results)
        {
            lblAttackerWinRate.Content = results.AttackerWinRate + "%";
            lblDefenderWinRate.Content = results.DefenderWinRate + "%";
            lblTieRate.Content = results.TieRate + "%";
            lblAttackerAverageIPCLost.Content = results.AverageAttackerIPCLost();
            lblDefenderAverageIPCLost.Content = results.AverageDefenderIPCLost();
            lblAttackerMedianIPCLost.Content = results.MedianAttackerIPCLost;
            lblDefenderMedianIPCLost.Content = results.MedianDefenderIPCLost;

            Debug.WriteLine("Defender Wins: " + results.DefenderWins + " Percentage: " + results.DefenderWins / results.TotalFights);
            Debug.WriteLine("Defender Average Losses: " + results.TotalDefenderIPCLost / results.TotalFights);
            Debug.WriteLine("Attacker Wins: " + results.AttackerWins + " Percentage: " + results.AttackerWins / results.TotalFights);
            Debug.WriteLine("Attacker Average Losses: " + results.TotalAttackerIPCLost / results.TotalFights);

            Debug.WriteLine("Attacker Median Losses: " + results.MedianAttackerIPCLost);
            Debug.WriteLine("Defender Median Losses: " + results.MedianDefenderIPCLost);


            /* Test Code adding a chart. This is left as a sample because the samples online are annoying
            Histogram h = results.AttackerLossesHistogram();
            CartesianMapper<Bucket> mapper = new CartesianMapper<Bucket>()
                .X(value => Math.Max(0, Math.Ceiling(value.LowerBound)))
                .Y(value => value.Count);
            var series = new LiveCharts.Wpf.LineSeries(mapper);
            series.Values = new ChartValues<Bucket>();
            for (int i= 0; i < 100; i++)
            {
                try
                {
                    var b = h.GetBucketOf(i);
                    if (b != null)
                    {
                        series.Values.Add(b);
                    }
                }
                catch (Exception ex) { }
            }

            AttackerChart.Series.Add(series);
            */
            Debug.WriteLine("========================================================");
        }
        public void OutputNavyStats(ResultsTracker results)
        {
            lblAttackerWinRateNavy.Content = results.AttackerWinRate + "%";
            lblDefenderWinRateNavy.Content = results.DefenderWinRate + "%";
            lblTieRateNavy.Content = results.TieRate + "%";
            lblAttackerAverageIPCLostNavy.Content = results.AverageAttackerIPCLost();
            lblDefenderAverageIPCLostNavy.Content = results.AverageDefenderIPCLost();
            lblStalemateRateNavy.Content = results.StalemateRate + "%";

            Debug.WriteLine("Defender Wins: " + results.DefenderWins + " Percentage: " + results.DefenderWins / results.TotalFights);
            Debug.WriteLine("Defender Average Losses: " + results.TotalDefenderIPCLost / results.TotalFights);
            Debug.WriteLine("Attacker Wins: " + results.AttackerWins + " Percentage: " + results.AttackerWins / results.TotalFights);
            Debug.WriteLine("Attacker Average Losses: " + results.TotalAttackerIPCLost / results.TotalFights);

            Debug.WriteLine("========================================================");
        }
        public void OutputWinningAttacker(Army attacker)
        {
            lblWinningAttacker.Text = attacker.ToString();
            Debug.WriteLine("Winning attacker: " + attacker.ToString());
        }
        public void NoWinningAttackerFound()
        {
            lblWinningAttacker.Text = "No winning attacker found";
            lblAttackerWinRate.Content = "";
            lblDefenderWinRate.Content = "";
            lblTieRate.Content = "";
            lblAttackerAverageIPCLost.Content = "";
            lblDefenderAverageIPCLost.Content = "";
            lblAttackerMedianIPCLost.Content = "";
            lblDefenderMedianIPCLost.Content = "";
            Debug.WriteLine("No winning attacker found at given certainty");
        }
        public int GetRounds()
        {
            return Int32.Parse(rounds.Text);            
        }
        public Army GetArmyAttackers()
        {
            Army attackers = new Army();
            //We are throwing on bad input. Way easier in V1
            int unitCount= Int32.Parse(attInf.Text);
            attackers.AddInfantry(unitCount);         

            unitCount = Int32.Parse(attTank.Text);
            attackers.AddTanks(unitCount);

            unitCount = Int32.Parse(attArt.Text);
            attackers.AddArtillery(unitCount);
           
            unitCount = Int32.Parse(attFight.Text);
            attackers.AddFighters(unitCount);

            unitCount = Int32.Parse(attBomb.Text);
            attackers.AddBombers(unitCount);

            return attackers;
        }
        public Army GetArmyDefenders()
        {
            Army defenders = new Army();
            //We are throwing on bad input. Way easier in V1
            Int32 unitCount = Int32.Parse(defInf.Text);
            defenders.AddInfantry(unitCount);

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

            return defenders;
        }
        public Fleet GetNavyAttackers()
        {
            Fleet attacker = new Fleet();            
            attacker.AddBattleships(int.Parse(attBattleship.Text));
            attacker.AddCruisers(int.Parse(attCruiser.Text));
            attacker.AddDestroyers(int.Parse(attDestroyer.Text));
            attacker.AddSubmarines(int.Parse(attSub.Text));
            attacker.AddAircraftCarriers(int.Parse(attAircraftCarrier.Text));
            attacker.AddFighters(int.Parse(attFighters.Text));
            attacker.AddBombers(int.Parse(attBombers.Text));
            return attacker;
        }
        public Fleet GetNavyDefenders()
        {
            Fleet defender = new Fleet();
            defender.AddBattleships(int.Parse(defBattleship.Text));
            defender.AddCruisers(int.Parse(defCruiser.Text));
            defender.AddDestroyers(int.Parse(defDestroyer.Text));
            defender.AddSubmarines(int.Parse(defSub.Text));
            defender.AddAircraftCarriers(int.Parse(defAircraftCarrier.Text));
            defender.AddFighters(int.Parse(defFighters.Text));
            defender.AddBombers(int.Parse(defBombers.Text));
            return defender;
        }
        public void ResetUI()
        {
            defBattleship.Text = "0";
            defCruiser.Text = "0";
            defDestroyer.Text = "0";
            defSub.Text = "0";
            defAircraftCarrier.Text = "0";
            defFighters.Text = "0";
            defBombers.Text = "0";

            attBattleship.Text = "0";
            attCruiser.Text = "0";
            attDestroyer.Text = "0";
            attSub.Text = "0";
            attAircraftCarrier.Text = "0";
            attFighters.Text = "0";
            attBombers.Text = "0";


            defInf.Text = "0";
            defArt.Text = "0";
            defAA.Text = "0";
            defTank.Text = "0";
            defFight.Text = "0";
            defBomb.Text = "0";

            attInf.Text = "0";
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
        private void NavyFight_Click(object sender, RoutedEventArgs e)
        {
            ProcessingFight();
            controller.NavyFight();
            FinishedProcessing();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            controller.FindMinAttackingArmy(95);
        }
    }
}
