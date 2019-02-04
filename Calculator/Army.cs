using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{

    public class Army
    {

        public List<Unit> Infantry;
        public List<Unit> SupportedInfantry;
        public List<Unit> Artillery;
        public List<Unit> Tanks;
        public List<Unit> AA;
        public List<Unit> Fighters;
        public List<Unit> Bombers;
        public int Losses;
                
        public void debug(string s)
        {
            //Debug.WriteLine(s);
        }

        public Army()
        {
            Infantry = new List<Unit>();
            SupportedInfantry = new List<Unit>();
            Artillery = new List<Unit>();
            Tanks = new List<Unit>();
            AA = new List<Unit>();
            Fighters = new List<Unit>();
            Bombers = new List<Unit>();
            Losses = 0;
        }
        public int RollDefence()
        {
            return RollGroundCombat(Posture.Defense);
        }

        public int RollOffense()
        {
            return RollGroundCombat(Posture.Attack);
        }
        private int RollGroundCombat(int posture)
        {
            int hitCount = 0;
            foreach (var infantry in Infantry)
            {
                if (infantry.doesHit(posture))
                {
                    debug("Inf Hit");
                    hitCount++;
                }
            }
            foreach (var supInf in SupportedInfantry)
            {
                if (supInf.doesHit(posture))
                {
                    debug("Sup Inf Hit");
                    hitCount++;
                }
            }

            foreach (var art in Artillery)
            {
                if (art.doesHit(posture))
                {
                    debug("Art Hit");
                    hitCount++;
                }
            }

            foreach (var tank in Tanks)
            {
                if (tank.doesHit(posture))
                {
                    debug("Tank Hit");
                    hitCount++;
                }
            }

            foreach (var fighter in Fighters)
            {
                if (fighter.doesHit(posture))
                {
                    debug("Figher Hit");
                    hitCount++;
                }
            }

            foreach (var bomber in Bombers)
            {
                if (bomber.doesHit(posture))
                {
                    debug("Bomber Hit");
                    hitCount++;
                }
            }
            return hitCount;
        }
        private void RebalanceSupportedInfantry()
        {
            if (Artillery.Count == 0 || Artillery.Count == SupportedInfantry.Count)
            {
                return; 
            }
            //We need to move the inf to sup inf, the problem is that we can't lose the test
            //state or it will screw up the unit tests. Maybe the best solution is to have a 
            //an upgrade/downgrade method on the inf unit to move between the two classes 
        }
        public int NumberOfRemainingUnits()
        {
            int totalUnits = 0;
            totalUnits += AA.Count;
            totalUnits += Infantry.Count;
            totalUnits += SupportedInfantry.Count;
            totalUnits += Artillery.Count;
            totalUnits += Tanks.Count;
            totalUnits += Bombers.Count;
            totalUnits += Fighters.Count;
            return totalUnits;
        }

        public void RemoveAirForce(int hitCount)
        {
            hitCount = RemoveAndReturnRemainder(Fighters, hitCount);
            //It handles 0 removes, so just go for it
            hitCount = RemoveAndReturnRemainder(Bombers, hitCount);
        }
        public void RemoveGroundForceDefender(int hitCount)
        {
            //It handles 0 removes, so just go for it
            hitCount = RemoveAndReturnRemainder(AA, hitCount);
            hitCount = RemoveAndReturnRemainder(Infantry, hitCount);
            hitCount = RemoveAndReturnRemainder(SupportedInfantry, hitCount);
            hitCount = RemoveAndReturnRemainder(Artillery, hitCount);
            hitCount = RemoveAndReturnRemainder(Tanks, hitCount);
            hitCount = RemoveAndReturnRemainder(Bombers, hitCount);
            hitCount = RemoveAndReturnRemainder(Fighters, hitCount);

        }
        public void RemoveGroundForceAttacker(int hitCount)
        {
            hitCount = RemoveAndReturnRemainder(AA, hitCount);
            hitCount = RemoveAndReturnRemainder(Infantry, hitCount);
            hitCount = RemoveAndReturnRemainder(SupportedInfantry, hitCount);
            hitCount = RemoveAndReturnRemainder(Artillery, hitCount);
            hitCount = RemoveAndReturnRemainder(Tanks, hitCount);
            hitCount = RemoveAndReturnRemainder(Fighters, hitCount);
            hitCount = RemoveAndReturnRemainder(Bombers, hitCount);
        }

        public bool CanStillFight()
        {
            if (Infantry.Count > 0)
            {
                return true;
            }
            if (SupportedInfantry.Count > 0)
            {
                return true;
            }
            if (Artillery.Count > 0)
            {
                return true;
            }
            if (Tanks.Count > 0)
            {
                return true;
            }
            if (Fighters.Count > 0)
            {
                return true;
            }
            if (Bombers.Count > 0)
            {
                return true;
            }
            return false;
        }
        private int RemoveAndReturnRemainder(List<Unit> units, int hitCount)
        {
            if (units.Count == 0)
            {
                return hitCount;
            }
            for (; hitCount > 0; hitCount--)
            {
                if (units.Count == 0)
                {
                    break;
                }
                Losses += units.ElementAt(0).IpcValue;
                units.RemoveAt(0);
            }
            return hitCount;
        }
        public override string ToString()
        {
            return "Inf: " + Infantry.Count + " Sup Inf: " + SupportedInfantry.Count +
                " AA: " + AA.Count +
                " Art: " + Artillery.Count + " Tanks:" + Tanks.Count +
                " Fighters: " + Fighters.Count + " Bombers: " + Bombers.Count;
        }


    }


}
