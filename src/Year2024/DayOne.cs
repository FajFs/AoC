namespace AoC.Year2024;
public partial class DayOne(
    ILogger<DayOne> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<DayOne> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));

    [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    private static partial Regex MatchDigit();

    private static int AbsolutValue(int x) 
        => x < 0 ? -x : x;

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 1);

        var leftList = input.Split('\n')
            .Select(x => MatchDigit().Matches(x).First().Value)
            .Select(int.Parse)
            .OrderByDescending(x => x);

        var rightList = input.Split('\n')
            .Select(x => MatchDigit().Matches(x).Last().Value)
            .Select(int.Parse)
            .OrderByDescending(x => x);

        var result = leftList
            .Zip(rightList, (l, r) => AbsolutValue(l - r))
            .Sum();

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 1);

        var leftList = input.Split('\n')
            .Select(x => MatchDigit().Matches(x).First().Value)
            .Select(int.Parse);

        var rightList = input.Split('\n')
            .Select(x => MatchDigit().Matches(x).Last().Value)
            .Select(int.Parse);

        var rightListGrouped = rightList
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        var result = leftList
            .Select(x => rightListGrouped.GetValueOrDefault(x, 0) * x)
            .Sum();

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
