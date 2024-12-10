using System.Diagnostics.Metrics;

namespace AoC.Year2024;

public partial class Day10(
    ILogger<Day10> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day10> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));


    private static List<(int x, int y)> FindLowestLocations(int[][] map)
    {
        var locations = new List<(int x, int y)>();
        for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
                if (map[y][x] == 0)
                    locations.Add((x, y));  

        return locations;
    }


    private static int DiscoverPathsToTopRecusive(int[][] map, int x, int y, int previousValue, bool discoverAllPaths = false)
    {
        // Check if we are out of bounds
        if (y < 0 || y >= map.Length || x < 0 || x >= map[0].Length)
            return 0;

        var currentValue = map[y][x];
        if (currentValue != previousValue + 1)
            return 0;

        if (discoverAllPaths && currentValue == 9)
            return 1;
        else
        {
            if (currentValue == -99)
                return 0;

            // Check if we reached the top, i.e., 9
            if (currentValue == 9)
            {
                // Mark as visited
                map[y][x] = -99;
                return 1;
            }
        }

        // Recursive calls
        return DiscoverPathsToTopRecusive(map, x, y - 1, currentValue, discoverAllPaths) // Go up
            + DiscoverPathsToTopRecusive(map, x, y + 1, currentValue, discoverAllPaths) // Go down
            + DiscoverPathsToTopRecusive(map, x - 1, y, currentValue, discoverAllPaths) // Go left
            + DiscoverPathsToTopRecusive(map, x + 1, y, currentValue, discoverAllPaths); // Go right
    }


    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 10);
        var map = input
            .Split('\n')
            .Select(line => line
                .Select(c => int.Parse(c.ToString())) 
                .ToArray()) 
            .ToArray();

        var result =
            FindLowestLocations(map)
            .Sum(location =>
            {
                var mapCopy = map.Select(x => x.ToArray()).ToArray();
                return DiscoverPathsToTopRecusive(mapCopy, location.x, location.y, -1);
            });

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 10);
        var map = input
            .Split('\n')
            .Select(line => line
                .Select(c => int.Parse(c.ToString()))
                .ToArray())
            .ToArray();

        var result =
            FindLowestLocations(map)
            .Sum(location => DiscoverPathsToTopRecusive(map, location.x, location.y, -1, discoverAllPaths: true));

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}


