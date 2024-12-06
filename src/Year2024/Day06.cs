using System.Diagnostics;

namespace AoC.Year2024;
public partial class Day06(
    ILogger<Day06> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<Day06> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));

    public record struct Velocity(int Dx, int Dy);
    private record struct Guard(int X, int Y, Velocity Velocity);

    private readonly char Start = '^';
    private readonly char Obstacle = '#';
    private readonly char Visited = 'X';
    private Guard GetGuardStartPoint(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
            for (int j = 0; j < map[i].Length; j++)
                if (map[i][j] == Start)
                    return new Guard(j, i, default);
        throw new InvalidOperationException("Start point not found in map");
    }

    private static Velocity TurnGuardVelocityRight(Velocity velocity) 
        => velocity switch
        {
            { Dx: 0, Dy: -1 } => new(1, 0),
            { Dx: 1, Dy: 0 } => new(0, 1),
            { Dx: 0, Dy: 1 } => new(-1, 0),
            { Dx: -1, Dy: 0 } => new (0, -1),
            _ => throw new InvalidOperationException("Invalid velocity")
        };

    private static int MapVelocityToIndex(Velocity velocity)
        => velocity switch
        {
            { Dx: 0, Dy: -1 } => 0,
            { Dx: 1, Dy: 0 } => 1,
            { Dx: 0, Dy: 1 } => 2,
            { Dx: -1, Dy: 0 } => 3,
            _ => throw new InvalidOperationException("Invalid velocity")
        };

    private (bool IsStuck, int visitedCount) SimulateGuardMovement(char[][] map, Guard startPoint)
    {
        var guard = startPoint with { Velocity = new Velocity(0, -1) };
        var visitedTiles = new bool[map.Length, map[0].Length, 4]; //4 is the number of possible velocities
        var visitedCount = 0;
        while (true)
        {
            var velocityIndex = MapVelocityToIndex(guard.Velocity);
            //if we have visited this point before, we are stuck
            if (visitedTiles[guard.Y, guard.X, velocityIndex])
                return (true, 0);

            visitedTiles[guard.Y, guard.X, velocityIndex] = true;
            if (map[guard.Y][guard.X] != Visited)
                visitedCount++;

            //mark the current point as visited
            map[guard.Y][guard.X] =  Visited;
            var nextX = guard.X + guard.Velocity.Dx;
            var nextY = guard.Y + guard.Velocity.Dy;

            //if we are out of bounds, we are not stuck
            if (nextY < 0 || nextY >= map.Length || nextX < 0 || nextX >= map[nextY].Length)
                return (false, visitedCount);

            if (map[nextY][nextX] == Obstacle)
            {
                guard.Velocity = TurnGuardVelocityRight(guard.Velocity);
            }
            else
            {
                guard.X = nextX;
                guard.Y = nextY;
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
        var timer = Stopwatch.StartNew();
        var input = await _client.GetInputAsync(2024, 6);
        var map = input.Split("\n")
            .Select(x => x.ToCharArray())
            .ToArray();

        //Do a dry run to find the path of the guard
        var startPoint = GetGuardStartPoint(map);
        SimulateGuardMovement(map, startPoint);

        var mapCopy = map.Select(row => row.ToArray()).ToArray();
        var result = 0;
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] != 'X')
                    continue;

                mapCopy[i][j] = Obstacle;
                var (isStuck, _) = SimulateGuardMovement(mapCopy, startPoint);
                mapCopy[i][j] = 'X';

                if (isStuck)
                    result++;
            }
        }

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
        _logger.LogInformation("Time: {time}", timer.Elapsed);
    }
}
