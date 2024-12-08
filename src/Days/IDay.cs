namespace AoC.Days;
public interface IDay
{
    public Task SolvePart1();
    public Task SolvePart2();
}

public static class DayExtensions
{
    public static async Task SolveAsync(this IDay day)
        =>  await Task.WhenAll(LogExecutionTimeAsync(
            day.SolvePart1, nameof(day.SolvePart1)), 
            LogExecutionTimeAsync(day.SolvePart2, nameof(day.SolvePart2)));

    public static async Task LogExecutionTimeAsync(Func<Task> func, string methodName)
    {
        var stopwatch = Stopwatch.StartNew();
        await func();
        stopwatch.Stop();
        Log.Information("{MethodName}: {ElapsedSeconds:F4} seconds",
            methodName,
            stopwatch.Elapsed.TotalSeconds);
    }
}