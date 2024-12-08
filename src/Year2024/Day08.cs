namespace AoC.Year2024;

public partial class Day08(
    ILogger<Day08> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day08> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));

    public record Antenna(char Type, int X, int Y);
    public record Distance(int X, int Y);

    private readonly char AntiNode = '$';

    private static List<Antenna> GetAntennas(char[][] map)
    {
        var antennas = new List<Antenna>();
        for (int i = 0; i < map.Length; i++)
            for (int j = 0; j < map[i].Length; j++)
                if (map[i][j] != '.')
                    antennas.Add(new Antenna(map[i][j], j, i));
        return antennas;
    }

    private static Distance GetDistance(Antenna a, Antenna b)
        => new(a.X - b.X, a.Y - b.Y);

    private static Distance GetReversedDistance(Antenna a, Antenna b)
    {
        var distance = GetDistance(a, b);
        return distance with { X = -distance.X, Y = -distance.Y };
    }

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 8);
        var map = input
            .Split('\n')
            .Select(x => x.ToCharArray())
            .ToArray();

        var antennasGroups = GetAntennas(map)
            .GroupBy(x => x.Type);

        foreach (var antennaGroup in antennasGroups)
            foreach (var antenna in antennaGroup)
            {
                var antiNodeAntennasLocations = antennaGroup
                    .Where(currentAntenna => currentAntenna != antenna)
                    .Select(currentAntenna => GetDistance(antenna, currentAntenna))
                    .Select(distance => new Antenna(AntiNode, antenna.X + distance.X, antenna.Y + distance.Y))
                    .Where(antiNodeAntenna => antiNodeAntenna.X >= 0 && antiNodeAntenna.X < map[0].Length && antiNodeAntenna.Y >= 0 && antiNodeAntenna.Y < map.Length)
                    .Distinct();

                //place the antiNodes on the map
                foreach (var antiNodeAntennasLocation in antiNodeAntennasLocations)
                    map[antiNodeAntennasLocation.Y][antiNodeAntennasLocation.X] = antiNodeAntennasLocation.Type;
            }

        var result = map
            .SelectMany(x => x)
            .Count(x => x == AntiNode);

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }


    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 8);
        var map = input
            .Split('\n')
            .Select(x => x.ToCharArray())
            .ToArray();

        var antennasGroups = GetAntennas(map)
            .GroupBy(x => x.Type);

        foreach (var antennaGroup in antennasGroups)
            foreach (var antenna in antennaGroup)
            {
                var antiNodeDistances = antennaGroup
                    .Where(currentAntenna => currentAntenna != antenna)
                    .SelectMany(currentAntenna => new[] { GetDistance(antenna, currentAntenna), GetReversedDistance(antenna, currentAntenna) })
                    .Distinct();

                foreach (var antiNodeDistance in antiNodeDistances)
                {
                    //get the next antiNode antenna
                    var nextAntiNodeAntenna = new Antenna(AntiNode, antenna.X + antiNodeDistance.X, antenna.Y + antiNodeDistance.Y);

                    //while the next antiNode antenna is within the map bounds, place the antiNode antennas on the map
                    while (nextAntiNodeAntenna.X >= 0 && nextAntiNodeAntenna.X < map[0].Length && nextAntiNodeAntenna.Y >= 0 && nextAntiNodeAntenna.Y < map.Length)
                    {
                        map[nextAntiNodeAntenna.Y][nextAntiNodeAntenna.X] = nextAntiNodeAntenna.Type;
                        nextAntiNodeAntenna = nextAntiNodeAntenna with { X = nextAntiNodeAntenna.X + antiNodeDistance.X, Y = nextAntiNodeAntenna.Y + antiNodeDistance.Y };
                    }
                }
            }

        var result = map
            .SelectMany(x => x)
            .Count(x => x == AntiNode);

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
