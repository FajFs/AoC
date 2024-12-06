using System.Diagnostics;

namespace AoC.Year2024;
public partial class Day06(
    ILogger<Day06> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<Day06> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));

    public record struct Direction(int X, int Y);
    private record struct Coordinate(int X, int Y, Direction? Direction = null);

    private readonly char Start = '^';
    private readonly char Obstacle = '#';

    private readonly char Visited = 'V';

    private Coordinate GetGuardStartPoint(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
            for (int j = 0; j < map[i].Length; j++)
                if (map[i][j] == Start)
                    return new Coordinate(j, i);
        throw new InvalidOperationException("Start point not found in map");
    }

    private static Direction TurnGuardRight(Direction direction) 
        => direction switch
        {
            { X: 0, Y: -1 } => direction with { X = 1, Y = 0 },
            { X: 1, Y: 0 } => direction with { X = 0, Y = 1 },
            { X: 0, Y: 1 } => direction with { X = -1, Y = 0 },
            { X: -1, Y: 0 } => direction with { X = 0, Y = -1 },
            _ => throw new InvalidOperationException("Invalid direction")
        };

    private (bool IsStuck, int visitedCount) SimulateGuardMovement(char[][] map, Coordinate startPoint)
    {
        var visited = new HashSet<Coordinate>();
        var direction = new Direction(0, -1);
        var guard = startPoint with { Direction = direction };

        while (true)
        {
            //if we have visited this point before, we are stuck
            if (visited.Add(guard) is false)
                return (true, 0);

            var guardLookAhead = new Coordinate(guard.X + direction.X, guard.Y + direction.Y);
            //if we are out of bounds, we are not stuck
            if (guardLookAhead.Y < 0 || guardLookAhead.Y >= map.Length || guardLookAhead.X < 0 || guardLookAhead.X >= map[guardLookAhead.Y].Length)
                return (false, visited.Select(Point => Point with { Direction = null }).ToHashSet().Count);

            //if we hit an obsticle, turn right
            if (map[guardLookAhead.Y][guardLookAhead.X] == Obstacle)
            {
                direction = TurnGuardRight(direction);
                guard = guard with { Direction = direction };
            }
            //otherwise move forward
            else
            {
                guard = guardLookAhead with { Direction = direction };
            }
        }
    }

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 6);
        var map = input.Split("\n")
            .Select(x => x.ToCharArray())
            .ToArray();
        
        var startPoint = GetGuardStartPoint(map);
        var result = SimulateGuardMovement(map, startPoint)
            .visitedCount;

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }

    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 6);
        var map = input.Split("\n")
            .Select(x => x.ToCharArray())
            .ToArray();

        var mapVariants = map
            .SelectMany((row, i) =>
                row.Select((character, j) => (i, j, character)))
            .Where(x => x.character != Start && x.character != Obstacle)
            .Select(x =>
            {
                var mapVariant = map.Select(row => row.ToArray()).ToArray();
                mapVariant[x.i][x.j] = Obstacle;
                return mapVariant;
            });

        var startPoint = GetGuardStartPoint(map);

        //if you cannot outsmart them with code, outsmart them with parallelism and brute force
        var tasks = mapVariants.Select(mapVariant =>
            Task.Run(() => SimulateGuardMovement(mapVariant, startPoint)));

        var result = (await Task.WhenAll(tasks))
            .Count(x => x.IsStuck);

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
