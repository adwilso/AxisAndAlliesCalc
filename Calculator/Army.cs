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

        private List<Unit> Infantry;
        private List<Unit> SupportedInfantry;
        private List<Unit> Artillery;
        private List<Unit> Tanks;
        private List<Unit> AA;
        private List<Unit> Fighters;
        private List<Unit> Bombers;
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
        public int NumberOfPlanes
        {
            get
            {
                return Fighters.Count + Bombers.Count;
            }
        }
        public int RollGroundDefense()
        {
            return RollGroundCombat(Posture.Defense);
        }
        public bool HasAA()
        {
            if (AA.Count > 0)
            {
                return true;
            }
            return false;
        }
        public int CountOfAA()
        {
            return AA.Count();
        }
        public void AddInfantry(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Infantry.Add(new Infantry(isTest, alwaysHit));
            }
        }
        public void AddSupportedInfantry(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                SupportedInfantry.Add(new SupportedInfantry(isTest, alwaysHit));
            }
        }
        public void AddArtillery(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Artillery.Add(new Artillery(isTest, alwaysHit));
            }
        }
        public void AddTanks(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Tanks.Add(new Tank(isTest, alwaysHit));
            }
        }
        public void AddAA(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                AA.Add(new AA(isTest, alwaysHit));
            }
        }
        public void AddFighters(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Fighters.Add(new Fighter(isTest, alwaysHit));
            }
        }
        public void AddBombers(int count, bool isTest = false, bool alwaysHit = false)
        {
            if (count < 1)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Bombers.Add(new Bomber(isTest, alwaysHit));
            }
        }
        public int RollAADefense(int numberOfPlanes)
        {
            int aaHitCount = 0;
            if (numberOfPlanes > 0)
            {
                
                foreach (var aa in AA)
                {
                    //3 rolls or one for each plane
                    for (int i = 0; i < numberOfPlanes && i < 3; i++)
                    {
                        if (aa.doesHit(Posture.Defense))
                        {
                            debug("AA Hit");
                            aaHitCount += 1;
                        }
                    }
                }
            }
            return aaHitCount;
        }
        public int RollGroundAttack()
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
        public void RemoveAAPostDefeat()
        {
            if (AA.Count > 0)
            {
                RemoveAndReturnRemainder(AA, AA.Count);
            }            
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
