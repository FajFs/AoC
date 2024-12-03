namespace AoC.Year2024;
public partial class Day03(
    ILogger<Day03> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<Day03> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));

    [GeneratedRegex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled)]
    public partial Regex MultiplicationRegex();

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\((\d+),(\d+)\)", RegexOptions.Compiled)]
    public partial Regex DoDontOrMultiplicationRegex();

    private static readonly string Do = "do()";
    private static readonly string Dont = "don't()";

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 3);
        var result = input.Split('\n')
            .SelectMany(line => MultiplicationRegex().Matches(line)
                .Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value)))
            .Sum();

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 3);
        var doDontOrMuls = input.Split('\n')
            .SelectMany(line => DoDontOrMultiplicationRegex().Matches(line));

        var canDo = true;
        var result = 0;
        foreach (var match in doDontOrMuls)
        {
            if (match.Value == Do)
                canDo = true;
            else if (match.Value == Dont)
                canDo = false;
            else if (canDo)
                result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
