using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class FleetOutcome : IOutcome
    {
        private Fleet FinalAttacker;
        private Fleet FinalDefender;
        private FleetOutcome() { }
        public static FleetOutcome Fight(Fleet attacker, Fleet defender)
        {
            FleetOutcome outcome = new FleetOutcome();
            //Submarines never submerge, if you are going to submerge, just don't 
            //add them to the fleet in the UI

            // Check if they can fight each other (eg. Not subs versus planes) 
            while (CanFightEachOther(attacker, defender))
            {  
                int attackerSubHits = 0;
                int defenderSubHits = 0;
                int attackerSurfaceHits = 0;
                int defenderSurfaceHits = 0;
                int attackerPlaneHits = 0;
                int defenderPlaneHits = 0;
                bool attackerUsedSurprise = false;
                bool defenderUsedSurprise = false;

                //Check if the submarines are able to suprise attack each 
                if (attacker.HasSubmarines() && !defender.HasDestroyer())
                {
                    //Roll the surprise attack 
                    attackerSubHits = attacker.RollSubmarineAttack();
                    attackerUsedSurprise = true;
                }
                if (defender.HasSubmarines() && !attacker.HasDestroyer())
                {
                    defenderSubHits = defender.RollSubmarineDefense();
                    defenderUsedSurprise = true;
                }
                //Assign sub damage to ships and not planes
                attacker.RemoveSubmarineHits(defenderSubHits);
                defender.RemoveSubmarineHits(attackerSubHits);
                //Make sure they can still fight each other
                if (!CanFightEachOther(attacker,defender))
                {
                    break;
                }            
                //Roll for damage to all units unless they are subs that surprise attacked
                attackerSurfaceHits = attacker.RollSurfaceNavalAttack();
                attackerPlaneHits = attacker.RollPlanesAttacker();
                attackerSubHits = (!attackerUsedSurprise) ? attacker.RollSubmarineAttack() : 0;

                defenderSurfaceHits = defender.RollSurfaceNavalDefense();
                defenderPlaneHits = defender.RollPlanesDefender();
                defenderSubHits = (!defenderUsedSurprise) ? defender.RollSubmarineDefense() : 0;
                
                //Assign damage to the correct units
                attacker.RemovePlaneHits(defenderPlaneHits, defender.HasDestroyer());
                attacker.RemoveSubmarineHits(defenderSubHits);
                attacker.RemoveSurfaceHitsAttacker(defenderSurfaceHits);

                defender.RemovePlaneHits(attackerPlaneHits, attacker.HasDestroyer());
                defender.RemoveSubmarineHits(attackerSubHits);
                defender.RemoveSurfaceHitsAttacker(attackerSubHits);
            }

            if (!defender.CanStillFight() && attacker.CanStillFight())
            {
                outcome.Winner = Posture.Attack;
            }
            else if (defender.CanStillFight() && !attacker.CanStillFight())
            {
                outcome.Winner = Posture.Defense;
            }
            else if (!defender.CanStillFight() && !attacker.CanStillFight())
            {
                outcome.Winner = Posture.None;
            }
            else if (!CanFightEachOther(attacker, defender))
            {
                outcome.Winner = Posture.Stalemate;
            }
            else
            {
                throw new Exception("We fucked up");
            }
            outcome.FinalAttacker = attacker;
            outcome.FinalDefender = defender;
            return outcome;
        }

        public static bool CanFightEachOther(Fleet attacker, Fleet defender)
        {
            //If one doesn't have any units, they can't fight
            if (!attacker.CanStillFight() || !defender.CanStillFight())
            {
                return false;
            }
            if ((attacker.HasSurfaceShips() || attacker.NumberOfPlanes() > 0) &&
                (defender.HasSurfaceShips() || defender.NumberOfPlanes() > 0))
            {
                //if they both have surface ships or planes, they can fight each other
                return true;
            }
            //At this point one of them only has subs (can still fight, but don't have
            // planes or surface ships. See if the other person only has planes)
            if (attacker.HasSubmarines() && (defender.HasSubmarines() 
                                            || (defender.HasDestroyer() && (defender.NumberOfPlanes() > 0))
                                            || defender.HasSurfaceShips()))
            {
                return true;
            }
            if (defender.HasSubmarines() && (attacker.HasSubmarines() 
                                            || (attacker.HasDestroyer() && (attacker.NumberOfPlanes() > 0))
                                            || attacker.HasSurfaceShips()))
            {
                return true;
            }
            //Defaulting out if they can't hit each other
            return false;
        }
        public double FinalAttackerLosses
        {
            get
            {
                return FinalAttacker.IPCLosses;
            }
        }
        public double FinalDefenderLosses
        {
            get
            {
                return FinalDefender.IPCLosses;
            }
        }
        public int Winner
        {
            get; private set;
        }
        public bool AttackerCanStillFight()
        {
            return FinalAttacker.CanStillFight();
        }
        public int AttackerRemainingUnits()
        {
            throw new NotImplementedException();
        }
        public bool DefenderCanStillFight()
        {
            return FinalDefender.CanStillFight();
        }
        public int DefenderRemainingUnits()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
