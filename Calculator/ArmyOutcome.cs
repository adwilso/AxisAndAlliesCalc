using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class ArmyOutcome : IOutcome
    {                
        private Army FinalAttacker;
        private Army FinalDefender;
        public int Winner
        {
            get; private set;
        }
        public double FinalAttackerLosses
        {
            get
            {
                return FinalAttacker.Losses;
            }
        }
        public double FinalDefenderLosses
        {
            get
            {
                return FinalDefender.Losses;
            }
        }
        public bool AttackerCanStillFight()
        {
            return FinalAttacker.CanStillFight();
        }
        public bool DefenderCanStillFight()
        {
            return FinalDefender.CanStillFight();
        }
        public int AttackerRemainingUnits()
        {
            return FinalAttacker.NumberOfRemainingUnits();
        }
        public int DefenderRemainingUnits()
        {
            return FinalDefender.NumberOfRemainingUnits();
        }
        //Hook the debug output so I can choose who gets to log
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
                throw new Exception("ToString In Outcome fucked up");
            }
            return retval;
        }
        public static IOutcome Fight(Army attackers, Army defenders)
        {
            ArmyOutcome outcome = new ArmyOutcome();
            //Pre combat - roll one dice for each attacking plane, up to 3 for each AA
            if (defenders.HasAA()  && defenders.CanStillFight())
            {
                int aaHitCount = defenders.RollAADefense(attackers.NumberOfPlanes);
                attackers.RemoveAirForce(aaHitCount);                
            }

            while (attackers.CanStillFight() && defenders.CanStillFight())
            {
                //Combat - round N. Fight!
                debug("Attacking Army");
                debug(attackers.ToString());
                debug("Defending Army");
                debug(defenders.ToString());
                debug("Defender Rolls");
                int defenderHitCount = defenders.RollGroundDefense();
                debug("Total: " + defenderHitCount);
                debug("Attacker Rolls");
                int attackerHitCount = attackers.RollGroundAttack();
                debug("Total: " + attackerHitCount);

                defenders.RemoveGroundForceDefender(attackerHitCount);
                attackers.RemoveGroundForceAttacker(defenderHitCount);
            }
            debug("Combat is over");
            if (!defenders.CanStillFight() && attackers.CanStillFight())
            {
                debug("Attackers Win");
                defenders.RemoveAAPostDefeat();
                outcome.Winner = Posture.Attack;
            }
            else if (defenders.CanStillFight() && !attackers.CanStillFight())
            {
                debug("Defenders Win");
                attackers.RemoveAAPostDefeat();
                outcome.Winner = Posture.Defense;
            }
            else if (!defenders.CanStillFight() && !attackers.CanStillFight())
            {
                debug("Tie");
                attackers.RemoveAAPostDefeat();
                defenders.RemoveAAPostDefeat();
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
}
