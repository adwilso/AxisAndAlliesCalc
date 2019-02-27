using Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorUnitTests
{
    public class OutcomeMock : IOutcome
    {
        public double AttackerIpcLosses { get; set; }

        public double DefenderIpcLosses { get; set; }

        public double DefenderIpcRemaining { get; set; }

        public double AttackerIpcRemaining { get; set; }

        public double AttackerNumberOfUnits { get; set; }

        public double DefenderNumberOfUnits { get; set; }

        public int Winner { get; set; }

        public bool AttackerCanStillFight { get; set; }

        public bool DefenderCanStillFight { get; set; }

        public int AttackerRemainingUnits { get; set; }

        public int DefenderRemainingUnits { get; set; }
    }
}
