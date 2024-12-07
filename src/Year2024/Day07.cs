
namespace AoC.Year2024;
public partial class Day07(
    ILogger<Day07> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day07> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));

    private static bool CanSolveEquationRec(long solution, List<long> equation, bool shouldConcatinate = false)
    {
        if (equation.Count == 1)
            return equation[0] == solution;

        var a = equation.First();
        var b = equation.Skip(1).First();

        var remainingEquation = equation.Skip(2); 
        var sumEquation = remainingEquation.Prepend(a + b).ToList();
        var mulEquation = remainingEquation.Prepend(a * b).ToList();
        var concatEquation = remainingEquation.Prepend(long.Parse($"{a}{b}")).ToList();

        return CanSolveEquationRec(solution, sumEquation, shouldConcatinate)
            || CanSolveEquationRec(solution, mulEquation, shouldConcatinate)
            || (shouldConcatinate && CanSolveEquationRec(solution, concatEquation, shouldConcatinate));
    }


    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 7);

        var equations = input
            .Split('\n')
            .Select(x => _helper.MatchDigit()
                .Matches(x)
                .Select(m => m.Value)
                .Select(long.Parse)
                .ToList());

        var result = equations
            .Where(x => CanSolveEquationRec(x.First(), x.Skip(1).ToList()))
            .Sum(x => x.First());

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }

    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 7);

        var equations = input
            .Split('\n')
            .Select(x => _helper.MatchDigit()
                .Matches(x)
                .Select(m => m.Value)
                .Select(long.Parse)
                .ToList())
            .ToList();

        //find the failing equation to reduce search space
        var result = equations
            .Where(x => CanSolveEquationRec(x.First(), x.Skip(1).ToList(), shouldConcatinate: true))
            .Sum(x => x.First());

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
