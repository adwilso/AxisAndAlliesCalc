using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Fleet
    {
        public List<Unit> Cruisers;
        public List<Unit> Destroyers;
        public List<Unit> Battleships;
        public List<Unit> AircraftCarriers;
        public List<Unit> Submarines;
        public List<Unit> Fighters;
        public List<Unit> Bombers;
        public int IPCLosses = 0;

        private void debug(string input)
        {
            System.Diagnostics.Debug.WriteLine(input);
        }
        public Fleet()
        {
            Cruisers = new List<Unit>();
            Destroyers = new List<Unit>();
            Battleships = new List<Unit>();
            AircraftCarriers = new List<Unit>();
            Submarines = new List<Unit>();
            Fighters = new List<Unit>();
            Bombers = new List<Unit>();
        }
        public int RollNavalDefense()
        {
            return RollNavalCombat(Posture.Defense);
        }
        public int RollNavalAttack()
        {
            return RollNavalCombat(Posture.Attack);
        }
        private int RollNavalCombat(int posture)
        {
            int hitCount = 0;
            foreach (var cruiser in Cruisers)
            {
                if (cruiser.doesHit(posture))
                {
                    debug("Cruiser Hit");
                    hitCount++;
                }
            }
            foreach (var destroyer in Destroyers)
            {
                if (destroyer.doesHit(posture))
                {
                    debug("Destroyer Hit");
                    hitCount++;
                }
            }
            foreach (var battleship in Battleships)
            {
                if (battleship.doesHit(posture))
                {
                    debug("Battleship Hit");
                    hitCount++;
                }
            }
            foreach (var aircraftCarrier in AircraftCarriers)
            {
                if (aircraftCarrier.doesHit(posture))
                {
                    debug("Aircraft Carrier Hit");
                    hitCount++;
                }
            }
            foreach (var sub in Submarines)
            {
                if (sub.doesHit(posture))
                {
                    debug("Sub Hit");
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
            return  hitCount;
        }
        public int NumberOfRemainingUnits()
        {
            int numberOfUnits = 0;
            numberOfUnits += Cruisers.Count;
            numberOfUnits += Destroyers.Count;
            numberOfUnits += Battleships.Count;
            numberOfUnits += AircraftCarriers.Count;
            numberOfUnits += Submarines.Count;
            numberOfUnits += Fighters.Count;
            numberOfUnits += Bombers.Count;
            return numberOfUnits;
        }
        public bool CanStillFight()
        {
            if (NumberOfRemainingUnits() > 0)
            {
                return true;
            }
            return false;
        }
        public void RemoveNavalForceAttacker(int hitCount)
        {
            //First take as many hits on the battleships as you can since those 
            //are free
            hitCount = DamageBattleshipAndReturnRemainder(Battleships, hitCount);
            //Hit the rest of the ships in order of me feeling like it
            hitCount = RemoveAndReturnRemainder(Cruisers, hitCount);
            hitCount = RemoveAndReturnRemainder(Destroyers, hitCount);
            hitCount = RemoveAndReturnRemainder(Submarines, hitCount);
            hitCount = RemoveAndReturnRemainder(Fighters, hitCount);
            hitCount = RemoveAndReturnRemainder(Bombers, hitCount);
            hitCount = RemoveAndReturnRemainder(AircraftCarriers, hitCount);
            hitCount = RemoveAndReturnRemainder(Battleships, hitCount);
        }
        public void RemoveNavalForceDefender(int hitCount)
        {
            //First take as many hits on the battleships as you can since those 
            //are free
            hitCount = DamageBattleshipAndReturnRemainder(Battleships, hitCount);
            //Hit the rest of the ships in order of me feeling like it
            hitCount = RemoveAndReturnRemainder(Cruisers, hitCount);
            hitCount = RemoveAndReturnRemainder(Destroyers, hitCount);
            hitCount = RemoveAndReturnRemainder(Submarines, hitCount);
            hitCount = RemoveAndReturnRemainder(Fighters, hitCount);
            hitCount = RemoveAndReturnRemainder(Bombers, hitCount);
            hitCount = RemoveAndReturnRemainder(AircraftCarriers, hitCount);
            hitCount = RemoveAndReturnRemainder(Battleships, hitCount);
        }
        private int DamageBattleshipAndReturnRemainder(List<Unit> battleships, int hitCount)
        {
            //Go through the list of ships, for anything that is a battleship, see if it is hit
            //if it isn't hit, then add the hit and remove one hit count
            foreach(Unit u in battleships)
            {
                if (u is Battleship battleship)
                {
                    if(battleship.isHit == false)
                    {
                        battleship.isHit = true;
                        hitCount--;
                    }
                }
            }
            return hitCount;            
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
                IPCLosses += units.ElementAt(0).IpcValue;
                units.RemoveAt(0);
            }
            return hitCount;
        }
    }
}
