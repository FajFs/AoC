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
    private record struct Guard(int X, int Y, Direction Direction = default);

    private readonly char Start = '^';
    private readonly char Obstacle = '#';

    private Guard GetGuardStartPoint(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
            for (int j = 0; j < map[i].Length; j++)
                if (map[i][j] == Start)
                    return new Guard(j, i);
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

    private (bool IsStuck, int visitedCount) SimulateGuardMovement(char[][] map, Guard startPoint)
    {
        var visitedTiles = new HashSet<Guard>();
        var guard = startPoint with { Direction = new Direction(0, -1) };

        while (true)
        {
            //if we have visited this point before, we are stuck
            if (visitedTiles.Add(guard) is false)
                return (true, 0);

            var guardNextPosition = new Guard(guard.X + guard.Direction.X, guard.Y + guard.Direction.Y);
            //if we are out of bounds, we are not stuck
            if (guardNextPosition.Y < 0 || guardNextPosition.Y >= map.Length || guardNextPosition.X < 0 || guardNextPosition.X >= map[guardNextPosition.Y].Length)
                return (false, visitedTiles.Select(Point => Point with { Direction = default }).ToHashSet().Count);

            guard = map[guardNextPosition.Y][guardNextPosition.X] == Obstacle
                //if we hit an obsticle, turn right
                ? guard = guard with { Direction = TurnGuardRight(guard.Direction) }
                //otherwise move forward
                : guard = guardNextPosition with { Direction = guard.Direction };
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

        //if you cannot outsmart them with code, beat them with parallelism and brute force
        var tasks = mapVariants.Select(mapVariant =>
            Task.Run(() => SimulateGuardMovement(mapVariant, startPoint)));

        var result = (await Task.WhenAll(tasks))
            .Count(x => x.IsStuck);

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
