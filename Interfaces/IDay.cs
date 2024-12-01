namespace AoC.Interfaces;
public interface IDay
{
    public Task SolvePart1();
    public Task SolvePart2();
}

public static class DayExtensions
{
    public static async Task SolveAsync(this IDay day)
    {
        await day.SolvePart1();
        await day.SolvePart2();
    }
}