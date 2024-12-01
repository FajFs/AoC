namespace AoC.Days;
public interface IDay
{
    public Task SolvePart1();
    public Task SolvePart2();
}

public static class DayExtensions
{
    public static async Task SolveAsync(this IDay day)
        => await Task.WhenAll(day.SolvePart1(), day.SolvePart2());
}