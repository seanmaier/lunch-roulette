namespace lunch_roulette;

public interface ILunchService
{
    List<Lunch> GetLunches();
    Lunch CreateLunch(DateTime date);
    bool ResetLunches();
}