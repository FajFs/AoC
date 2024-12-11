namespace AoC.Year2024;

public partial class Day11(
    ILogger<Day11> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day11> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));

    private static long CountStones(Dictionary<long, long> currentStonesDictionary, int iterations)
    {
        foreach (var _ in Enumerable.Range(0, iterations))
        {
            var nextStonesDictionary = new Dictionary<long, long>();
            foreach (var (stone, frequency) in currentStonesDictionary)
            {
                var currentStoneString = stone.ToString();
                if (stone == 0)
                {
                    nextStonesDictionary[1] = nextStonesDictionary.TryGetValue(1, out long value) 
                        ? value + frequency 
                        : frequency;
                }
                else if (currentStoneString.Length % 2 == 0)
                {
                    var half = currentStoneString.Length / 2;
                    var firstHalf = long.Parse(currentStoneString[..half]);
                    var secondHalf = long.Parse(currentStoneString[half..]);

                    nextStonesDictionary[firstHalf] = nextStonesDictionary.TryGetValue(firstHalf, out long firstHalfValue) 
                        ? firstHalfValue + frequency 
                        : frequency;
                    nextStonesDictionary[secondHalf] = nextStonesDictionary.TryGetValue(secondHalf, out long secondHalfValue) 
                        ? secondHalfValue + frequency 
                        : frequency;
                }
                else
                {
                    var newStone = stone * 2024;
                    nextStonesDictionary[newStone] = nextStonesDictionary.TryGetValue(newStone, out long value) 
                        ? value + frequency 
                        : frequency;
                }
            }
            currentStonesDictionary = nextStonesDictionary;
        }
        return currentStonesDictionary.Values.Sum();
    }

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 11);
        var stones = input
            .Split(" ")
            .Select(long.Parse)
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        var result = CountStones(stones, 25);

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 11);
        var stones = input
            .Split(" ")
            .Select(long.Parse)
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        var result = CountStones(stones, 75);

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
