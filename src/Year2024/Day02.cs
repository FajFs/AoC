﻿namespace AoC.Year2024;
public partial class Day02(
    ILogger<Day02> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day02> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));

    public bool IsSafeDifference(int a, int b)
    {
        var diff = _helper.AbsolutValue(a - b);
        return diff >= 1 && diff <= 3;
    }

    public static bool IsAscending(int a, int b)
        => a < b;

    private bool ValidateReport(List<int> report)
    {
        var initialLevelDirection = IsAscending(report[0], report[1]);
        for (var i = 0; i < report.Count - 1; i++)
            if (IsSafeDifference(report[i], report[i + 1]) is false || initialLevelDirection != IsAscending(report[i], report[i + 1]))
                return false;
        return true;
    }

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 2);
        var reports = input.Split('\n')
            .Select(line => _helper.MatchDigit()
                .Matches(line)
                .Select(x => int.Parse(x.Value))
                .ToList()
            );

        var result = reports
            .Count(ValidateReport);

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }

    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 2);
        var reports = input.Split('\n')
            .Select(line => _helper.MatchDigit()
                .Matches(line)
                .Select(x => int.Parse(x.Value))
                .ToList()
            );

        var result = reports
            .Count(report =>
                ValidateReport(report) 
                || report
                    .Select((_, index) => report.Where((_, i) => i != index).ToList())
                    .Any(ValidateReport));

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
