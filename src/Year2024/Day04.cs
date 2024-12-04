namespace AoC.Year2024;
public partial class Day04(
    ILogger<Day04> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<Day04> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));


    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 4);

        var map = input
            .Split("\n")
            .Select(x => x.ToCharArray())
            .ToArray();

        var result = 0;
        for (var x = 0; x < map.Length; x++)
            for (var y = 0; y < map[x].Length; y++)
            {
                //skip if not X
                if (map[x][y] != 'X')
                    continue;

                //check right for XMAS
                if (y + 3 < map[x].Length && map[x][y + 1] == 'M' && map[x][y + 2] == 'A' && map[x][y + 3] == 'S')
                    result++;

                //check left for XMAS
                if (y - 3 >= 0 && map[x][y - 1] == 'M' && map[x][y - 2] == 'A' && map[x][y - 3] == 'S')
                    result++;

                //check down for XMAS
                if (x + 3 < map.Length && map[x + 1][y] == 'M' && map[x + 2][y] == 'A' && map[x + 3][y] == 'S')
                    result++;

                //check up for XMAS
                if (x - 3 >= 0 && map[x - 1][y] == 'M' && map[x - 2][y] == 'A' && map[x - 3][y] == 'S')
                    result++;

                //check diagonal right down for XMAS
                if (x + 3 < map.Length && y + 3 < map[x].Length && map[x + 1][y + 1] == 'M' && map[x + 2][y + 2] == 'A' && map[x + 3][y + 3] == 'S')
                    result++;

                //check diagonal left down for XMAS
                if (x + 3 < map.Length && y - 3 >= 0 && map[x + 1][y - 1] == 'M' && map[x + 2][y - 2] == 'A' && map[x + 3][y - 3] == 'S')
                    result++;

                //check diagonal right up for XMAS
                if (x - 3 >= 0 && y + 3 < map[x].Length && map[x - 1][y + 1] == 'M' && map[x - 2][y + 2] == 'A' && map[x - 3][y + 3] == 'S')
                    result++;

                //check diagonal left up for XMAS
                if (x - 3 >= 0 && y - 3 >= 0 && map[x - 1][y - 1] == 'M' && map[x - 2][y - 2] == 'A' && map[x - 3][y - 3] == 'S')
                    result++;
            }

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 4);

        var map = input
            .Split("\n")
            .Select(x => x.ToCharArray())
            .ToArray();

        var result = 0;
        for (var x = 0; x < map.Length; x++)
            for (var y = 0; y < map[x].Length; y++)
            {
                //skip if not A
                if (map[x][y] != 'A')
                    continue;

                var masCounter = 0;
                if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1][y - 1] == 'M' && x + 1 < map.Length && y + 1 < map[x].Length && map[x + 1][y + 1] == 'S') 
                    masCounter++;

                if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1][y - 1] == 'S' && x + 1 < map.Length && y + 1 < map[x].Length && map[x + 1][y + 1] == 'M')
                    masCounter++;

                if (x - 1 >= 0 && y + 1 < map[x].Length && map[x - 1][y + 1] == 'M' && x + 1 < map.Length && y - 1 >= 0 && map[x + 1][y - 1] == 'S')
                    masCounter++;

                if (x - 1 >= 0 && y + 1 < map[x].Length && map[x - 1][y + 1] == 'S' && x + 1 < map.Length && y - 1 >= 0 && map[x + 1][y - 1] == 'M')
                    masCounter++;

                if (masCounter >= 2)
                    result++;
            }

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
