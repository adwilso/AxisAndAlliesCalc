using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class SimulatorController
    {
        private MainWindow _window;
        public SimulatorController(MainWindow mainWindow)
        {
            _window = mainWindow;
        }
        public void ArmyFight()
        {
            Army attackers = new Army();
            Army defenders = new Army();
            int rounds = _window.GetRounds();

            ResultsTracker results = new ResultsTracker();
            for (int i = 0; i < rounds; i++)
            {
                attackers = _window.GetArmyAttackers();
                defenders = _window.GetArmyDefenders();
                var outcome = ArmyOutcome.Fight(attackers, defenders);
                results.AddOutcome(outcome);
            }

            _window.OutputArmyStats(results);
        }
        internal void NavyFight()
        {
            Fleet attackers;
            Fleet defenders;
            int rounds = _window.GetRounds();

            ResultsTracker results = new ResultsTracker();
            for (int i = 0; i < rounds; i++)
            {
                attackers = _window.GetNavyAttackers();
                defenders = _window.GetNavyDefenders();
                var outcome = FleetOutcome.Fight(attackers, defenders);
                results.AddOutcome(outcome);
            }
            _window.OutputNavyStats(results);
        }
        public void FindMinAttackingArmy(double certainty)
        {
            Army pristeneAttacker = _window.GetArmyAttackers();
            int maxNumberOfUnits = pristeneAttacker.NumberOfRemainingUnits();
            int rounds = _window.GetRounds();
            for (int numUnitsThisAttack = 1;
                numUnitsThisAttack < maxNumberOfUnits;
                numUnitsThisAttack++)
            {
                ResultsTracker results = new ResultsTracker();
                for (int i = 0; i < rounds; i++)
                {
                    Army defender = _window.GetArmyDefenders();
                    Army attacker = _window.GetArmyAttackers();
                    attacker.ReduceToCheapestAttacker(numUnitsThisAttack);
                    var outcome = ArmyOutcome.Fight(attacker, defender);
                    results.AddOutcome(outcome);
                }
                if (results.AttackerWinRate > certainty)
                {
                    pristeneAttacker.ReduceToCheapestAttacker(numUnitsThisAttack);
                    _window.OutputArmyStats(results);
                    _window.OutputWinningAttacker(pristeneAttacker);
                    return;
                }
            }
            _window.NoWinningAttackerFound();
            

            //given an army with K units
            //for i=0; i<k; i++
            //  Create a regular defender
            //  Create attacker with cheapest/best K units available
            //  Attack with I units
            //  Check if success >95%
            //      if not keep going

            //If we ever succeeded, return the min set
            //else fail out
        }
    }
}
