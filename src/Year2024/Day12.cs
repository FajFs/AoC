namespace AoC.Year2024;

public partial class Day12(
    ILogger<Day12> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day12> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 12);

        //_logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 12);

        //_logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
