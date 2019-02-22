using System;
using System.Collections.Generic;
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
                results.Outcomes.Add(outcome);
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
                results.Outcomes.Add(outcome);
            }
            _window.OutputNavyStats(results);
        }
    }
}
