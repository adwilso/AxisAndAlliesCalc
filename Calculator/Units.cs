using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class Unit
    {
        protected int attack;
        protected int defence;
        protected static Random rnd = new Random();
        protected int RollDice()
        {
            int result;

            result = rnd.Next(1, 7);
            return result;
        }
        public bool doesHit(int stance)
        {
            int diceValue = RollDice();
            if (stance == Posture.Attack && diceValue <= attack)
            {
                return true;
            }
            if (stance == Posture.Defense && diceValue <= defence)
            {
                return true;
            }
            return false;
        }
    }

    public class Infantry : Unit
    {
        public Infantry()
        {
            attack = 1;
            defence = 2;
        }
    }
    public class SupportedInfantry : Unit
    {
        public SupportedInfantry()
        {
            attack = 2;
            defence = 2;
        }
    }
    public class Artillery : Unit
    {
        public Artillery()
        {
            attack = 2;
            defence = 2;
        }
    }
    public class Tank : Unit
    {
        public Tank()
        {
            attack = 3;
            defence = 3;
        }
    }
    public class AA : Unit
    {
        public AA()
        {
            attack = 0;
            defence = 1;
        }
    }
    public class Fighter : Unit
    {
        public Fighter()
        {
            attack = 3;
            defence = 4;

        }
    }
    public class Bomber : Unit
    {
        public Bomber()
        {
            attack = 4;
            defence = 1;
        }
    }
}
