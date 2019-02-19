using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Fleet
    {
        private List<Unit> Cruisers;
        private List<Unit> Destroyers;
        private List<Unit> Battleships;
        private List<Unit> AircraftCarriers;
        private List<Unit> Submarines;
        private List<Unit> Fighters;
        private List<Unit> Bombers;
        public int IPCLosses { get; private set; } 

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
        public void AddCruisers (int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Cruisers.Add(new Cruiser(isTest, alwaysHit));
            }
        }
        public void AddDestroyers(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Destroyers.Add(new Destroyer(isTest, alwaysHit));
            }
        }
        public void AddBattleships(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Battleships.Add(new Battleship(isTest, alwaysHit));
            }
        }
        public void AddAircraftCarriers(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                AircraftCarriers.Add(new AircraftCarrier(isTest, alwaysHit));
            }
        }
        public void AddSubmarines(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Submarines.Add(new Submarine(isTest, alwaysHit));
            }
        }
        public void AddFighters(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1 || !CanAddPlanes(count))
            {
                throw new Exception("Can't add more planes"); 
            }
            for (int i = 0; i < count; i++)
            {
                Fighters.Add(new Fighter(isTest, alwaysHit));
            }
        }
        public void AddBombers(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1 || !CanAddPlanes(count))
            {
                throw new Exception("Can't add more planes");                
            }
            for (int i = 0; i < count; i++)
            {
                Bombers.Add(new Bomber(isTest, alwaysHit));
            }
        }
        public bool HasDestroyer()
        {
            if (Destroyers.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool HasSubmarines()
        {
            if (Submarines.Count > 0)
            {
                return true;
            }
            return false;
        }
        public int NumberOfPlanes()
        {
            return Bombers.Count + Fighters.Count;
        }
        public bool CanAddPlanes(int planesToAdd)
        {
            if (planesToAdd <= 0)
            {
                return false;
            }
            int maxPlanes = AircraftCarriers.Count * AircraftCarrier.Capacity;
            int currentPlanes = NumberOfPlanes();
            if (maxPlanes >= currentPlanes + planesToAdd)
            {
                return true;
            }
            return false;
        }
        public int RollSubmarineAttack()
        {
            return RollSubmarine(Posture.Attack);
        }
        public int RollSubmarineDefense()
        {
            return RollSubmarine(Posture.Defense);
        }
        private int RollSubmarine(int posture)
        {
            int hitCount = 0;
            foreach (var sub in Submarines)
            {
                if (sub.doesHit(posture))
                {
                    debug("Surprise Sub Hit");
                    hitCount++;
                }
            }
            return hitCount;
        }
        public int RollPlanesAttacker()
        {
            return RollPlaneDamage(Posture.Attack);
        }
        public int RollPlanesDefender()
        {
            return RollPlaneDamage(Posture.Defense);
        }
        private int RollPlaneDamage(int posture)
        {
            int hitCount = 0;
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
        public int RollSurfaceNavalDefense()
        {
            return RollSurfaceNavalCombat(Posture.Defense);
        }
        public int RollSurfaceNavalAttack()
        {
            return RollSurfaceNavalCombat( Posture.Attack);
        }
        private int RollSurfaceNavalCombat(int posture)
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
        public bool HasSurfaceShips()
        {
            if (Cruisers.Count + Destroyers.Count + AircraftCarriers.Count + Battleships.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool CanStillFight()
        {
            if (NumberOfRemainingUnits() > 0)
            {
                return true;
            }
            return false;
        }
        public void RemoveSubmarineHits(int hitCount)
        {
            // First take as many hits on the battleships as you can since those
            //are free
            hitCount = DamageBattleshipAndReturnRemainder(Battleships, hitCount);
            //Hit the rest of the ships in order of me feeling like it
            hitCount = RemoveAndReturnRemainder(Cruisers, hitCount);
            hitCount = RemoveAndReturnRemainder(Destroyers, hitCount);
            hitCount = RemoveAndReturnRemainder(Submarines, hitCount);
            hitCount = RemoveAndReturnRemainder(AircraftCarriers, hitCount);
            hitCount = RemoveAndReturnRemainder(Battleships, hitCount);
        }
        public void RemovePlaneHits(int hitCount, bool hasDestroyer)
        {
            hitCount = RemoveAndReturnRemainder(Fighters, hitCount);
            hitCount = RemoveAndReturnRemainder(Bombers, hitCount);
            hitCount = DamageBattleshipAndReturnRemainder(Battleships, hitCount);
            hitCount = RemoveAndReturnRemainder(Cruisers, hitCount);
            hitCount = RemoveAndReturnRemainder(Destroyers, hitCount);
            hitCount = RemoveAndReturnRemainder(Submarines, hitCount);
            hitCount = RemoveAndReturnRemainder(AircraftCarriers, hitCount);
            hitCount = RemoveAndReturnRemainder(Battleships, hitCount);
            if (hasDestroyer)
            {
                hitCount = RemoveAndReturnRemainder(Submarines, hitCount);
            }
        }
        public void RemoveSurfaceHitsAttacker(int hitCount)
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
        public void RemoveSurfaceHitsDefender(int hitCount)
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
