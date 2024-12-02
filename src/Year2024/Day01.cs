namespace AoC.Year2024;
public class Day01(
    ILogger<Day01> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<Day01> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 1);

        var lerftRight = input.Split('\n')
            .Select(line =>
            {
                var matches = AocHelpers.MatchDigit().Matches(line).ToArray();
                return (
                    Left: int.Parse(matches.First().Value),
                    Right: int.Parse(matches.Last().Value)
                );
            }).ToList();

        var leftList = lerftRight
            .Select(x => x.Left)
            .OrderByDescending(x => x);

        var rightList = lerftRight
            .Select(x => x.Right)
            .OrderByDescending(x => x);

        var result = leftList
            .Zip(rightList, (l, r) => AocHelpers.AbsolutValue(l - r))
            .Sum();

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 1);

        var leftRight = input.Split('\n')
            .Select(line =>
            {
                var matches = AocHelpers.MatchDigit().Matches(line).ToArray();
                return (
                    Left: int.Parse(matches.First().Value),
                    Right: int.Parse(matches.Last().Value)
                );
            }).ToList();

        var rightListGrouped = leftRight
            .Select(x => x.Right)
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        var result = leftRight
            .Select(x => x.Left)
            .Select(x => rightListGrouped.GetValueOrDefault(x, 0) * x)
            .Sum();

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
