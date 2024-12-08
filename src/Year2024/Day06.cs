using System.Diagnostics;

namespace AoC.Year2024;
public partial class Day06(
    ILogger<Day06> _logger,
    AdventOfCodeClient _client)
    : IDay
{
    private readonly ILogger<Day06> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));

    private record struct Guard(int X, int Y);

    private readonly char Start = '^';
    private readonly char Obstacle = '#';
    private readonly char Visited = 'X';
    private Guard GetGuardStartPoint(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
            for (int j = 0; j < map[i].Length; j++)
                if (map[i][j] == Start)
                    return new Guard(j, i);
        throw new InvalidOperationException("Start point not found in map");
    }

    private static int TurnGruardRight(int velocity)
        => velocity switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            3 => 0,
            _ => throw new InvalidOperationException("Invalid velocity")
        };

    private static (int dx, int dy) MapIndexToVelocity(int velocity)
        => velocity switch
        {
            0 => (0, -1),
            1 => (1, 0),
            2 => (0, 1),
            3 => (-1, 0),
            _ => throw new InvalidOperationException("Invalid velocity")
        };


    private bool SimulateGuardMovement(char[][] map, Guard guard)
    {
        var velocity = 0; //0 up, 1 right, 2 down, 3 left
        var visitedTilesWithVelocity = new bool[map.Length, map[0].Length, 4]; //4 is the number of possible velocities
        while (true)
        {
            //if we have visited this point before, we are stuck
            if (visitedTilesWithVelocity[guard.Y, guard.X, velocity])
                return true;

            visitedTilesWithVelocity[guard.Y, guard.X, velocity] = true;

            //mark the current point as visited
            map[guard.Y][guard.X] = Visited;

            var (dx, dy) = MapIndexToVelocity(velocity);
            var nextX = guard.X + dx;
            var nextY = guard.Y + dy;

            //if we are out of bounds, we are not stuck
            if (nextY < 0 || nextY >= map.Length || nextX < 0 || nextX >= map[nextY].Length)
                return false;

            if (map[nextY][nextX] == Obstacle)
                velocity = TurnGruardRight(velocity);
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
        SimulateGuardMovement(map, startPoint);
        var result = map.Sum(x => x.Count(y => y == Visited));

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }

    public async Task SolvePart2()
    {
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
                if (map[i][j] != Visited)
                    continue;

                mapCopy[i][j] = Obstacle;
                result += SimulateGuardMovement(mapCopy, startPoint) ? 1 : 0;
                mapCopy[i][j] = Visited;
            }
        }
        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
