using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class FleetOutcome : IOutcome
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
            
            //Check if the submarines are able to suprise attack each other
                //Roll the surprise attack 
                //Assign sub damage to ships and not planes
            
            //Make sure they can still fight each other
            //Roll for damage to all units unless they are subs that surprise attacked
            //Assign damage to the correct un


            //Remove the transports from the game 

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
            // planes or surface ships)
            

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
            throw new NotImplementedException();
        }
        public int AttackerRemainingUnits()
        {
            throw new NotImplementedException();
        }
        public bool DefenderCanStillFight()
        {
            throw new NotImplementedException();
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
