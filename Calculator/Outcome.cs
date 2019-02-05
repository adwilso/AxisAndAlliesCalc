using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{

    public class Outcome
    {

        public int Winner = Posture.None;
        public Army FinalAttacker;
        public Army FinalDefender;

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

}
