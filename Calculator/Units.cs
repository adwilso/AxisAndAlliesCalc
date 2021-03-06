﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    /// <summary>
    /// Detects if we are running inside a unit test.
    /// 
    /// I know this is horrifying, but I need a way to make sure that we aren't running
    /// test code outside of the unit test
    /// 
    /// Entirely stolen from StackOverflow because there is no way I'd have thought of this
    /// </summary>
    public static class UnitTestDetector
    {
        static UnitTestDetector()
        {
            string testAssemblyName = "Microsoft.VisualStudio.TestPlatform.TestFramework";
            UnitTestDetector.IsInUnitTest = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith(testAssemblyName));
        }

        public static bool IsInUnitTest { get; private set; }
    }
    public class Posture
    {
        public const int None = 0;
        public const int Defense = 1;
        public const int Attack = 2;
        public const int Stalemate = 3;
    }
    public abstract class Unit
    {
        protected int attack;
        protected int defence;
        protected bool isTest = false;
        protected bool alwaysHit = false;
        protected static Random rnd = new Random();
        public Unit()
        {
            //Do nothing
        }
        public Unit(bool setIsTest, bool setAlwaysHit)
        {
            IsTest = setIsTest;
            AlwaysHit = setAlwaysHit;
        }
        public int IpcValue { get; set; }

        public bool IsTest { get { return isTest; }
            set 
            {
                if (value  == true)
                {
                    if (!UnitTestDetector.IsInUnitTest)
                    {
                        throw new Exception("Test value set outside test environment");
                    }
                }
                isTest = value; 
            }
        }
        public bool AlwaysHit
        {
            get { return alwaysHit; }
            set
            {
                if (value == true)
                {
                    if (!UnitTestDetector.IsInUnitTest)
                    {
                        throw new Exception("Test value set outside test environment");
                    }
                }
                alwaysHit = value;
            }
        }

        protected int RollDice()
        {
            int result;

            result = rnd.Next(1, 7);
            return result;
        }
        public bool doesHit(int posture)
        {
            //During tests, we should control if it hits 
            if (IsTest == true)
            {
                if (alwaysHit == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            //Do the math to see if we should hit
            int diceValue = RollDice();
            if (posture == Posture.Attack && diceValue <= attack)
            {
                return true;
            }
            if (posture == Posture.Defense && diceValue <= defence)
            {
                return true;
            }
            return false;
        }
    }

    public class Infantry : Unit
    {
        public Infantry(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 1;
            defence = 2;
            IpcValue = 3;
        }
    }
    public class SupportedInfantry : Unit
    {
        public SupportedInfantry(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 2;
            defence = 2;
            IpcValue = 3;
        }
    }
    public class Artillery : Unit
    {
        public Artillery(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 2;
            defence = 2;
            IpcValue = 4;
        }
    }
    public class Tank : Unit
    {
        public Tank(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 3;
            defence = 3;
            IpcValue = 6;
        }
    }
    public class AA : Unit
    {
        public AA(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 0;
            defence = 1;
            IpcValue = 5;
        }
    }
    public class Fighter : Unit
    {
        public Fighter(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 3;
            defence = 4;
            IpcValue = 10;
        }
    }
    public class Bomber : Unit
    {
        public Bomber(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 4;
            defence = 1;
            IpcValue = 12;
        }
    }
    public class Destroyer : Unit
    {
        public Destroyer(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 2;
            defence = 2;
            IpcValue = 8;
        }
    }
    public class Cruiser : Unit
    {
        public Cruiser(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 3;
            defence = 3;
            IpcValue = 12;
        }
    }
    public class AircraftCarrier : Unit
    {
        public AircraftCarrier(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 1;
            defence = 2;
            IpcValue = 14;
        }

        public static int Capacity = 2;
    }
    public class Battleship : Unit
    {
        public bool isHit = false;
        public Battleship()
        {
            attack = 4;
            defence = 4;
            IpcValue = 20;
        }

        public Battleship(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 4;
            defence = 4;
            IpcValue = 20;
        }
    }
    public class Submarine : Unit
    {
        public Submarine()
        {
            attack = 2;
            defence = 1;
            IpcValue = 6;
        }

        public Submarine(bool setIsTest, bool setAlwaysHit) : base(setIsTest, setAlwaysHit)
        {
            attack = 2;
            defence = 1;
            IpcValue = 6;
        }
    }

}
