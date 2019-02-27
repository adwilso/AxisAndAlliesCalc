namespace Calculator
{
    public interface IOutcome
    {
        double AttackerIpcLosses { get; }
        double DefenderIpcLosses { get; }
        double DefenderIpcRemaining { get; }
        double AttackerIpcRemaining { get; }
        double AttackerNumberOfUnits { get; }
        double DefenderNumberOfUnits { get; }
        int Winner { get; }
        bool AttackerCanStillFight { get; }
        bool DefenderCanStillFight { get; }
        int AttackerRemainingUnits { get; }
        int DefenderRemainingUnits { get; }
        string ToString();
    }
}