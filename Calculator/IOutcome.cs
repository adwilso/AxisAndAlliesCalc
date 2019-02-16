namespace Calculator
{
    public interface IOutcome
    {
        double FinalAttackerLosses { get; }
        double FinalDefenderLosses { get; }
        int Winner { get; }
        bool AttackerCanStillFight();
        bool DefenderCanStillFight();
        int AttackerRemainingUnits();
        int DefenderRemainingUnits();
        string ToString();
    }
}