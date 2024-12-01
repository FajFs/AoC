using AoC.Days;

namespace AoC.Year2024;
public partial class DayTwo(
    ILogger<DayTwo> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<DayTwo> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 2);

        //_logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 2);

        //_logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
