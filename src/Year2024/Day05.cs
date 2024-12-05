namespace AoC.Year2024;
public partial class Day05(
    ILogger<Day05> _logger,
    AdventOfCodeClient _client,
    AocHelper _helper)
    : IDay
{
    private readonly ILogger<Day05> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly AdventOfCodeClient _client = _client ?? throw new ArgumentNullException(nameof(_client));
    private readonly AocHelper _helper = _helper ?? throw new ArgumentNullException(nameof(_helper));

    [GeneratedRegex(@"(\d+)\|(\d+)", RegexOptions.Compiled)]
    public partial Regex RulePairRegex();

    private static bool EnsureValidOrder(List<(int before, int after)> orderingRules, List<int> update)
    {
        for (int i = 0; i < update.Count; i++)
        {
            var currentPage = update.ElementAt(i);
            
            var dependentRules = orderingRules.Where(rule => rule.after == currentPage);
            foreach (var (beforeRule, _) in dependentRules)
            {
                int expectedBeforePage = beforeRule;
                int expectedBeforeIndex = update.IndexOf(expectedBeforePage);
                if (expectedBeforeIndex > i)
                {
                    //swap the two elements and return false
                    update[expectedBeforeIndex] = currentPage;
                    update[i] = expectedBeforePage;
                    return false;
                }
            }
        }
        return true;
    }

    public async Task SolvePart1()
    {
        var input = await _client.GetInputAsync(2024, 5);

        var groups = input.Split("\n\n");
        var orderingRules = groups.First()
            .Split("\n")
            .Select(x => RulePairRegex().Match(x))
            .Select(x => (int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value)))
            .ToList();

        var result = groups.Last()
            .Split("\n")
            .Select(x => x.Split(",").Select(int.Parse).ToList())
            .Where(x => EnsureValidOrder(orderingRules, x))
            .Select(x => x.ElementAt(x.Count / 2))
            .Sum();

        _logger.LogInformation("{part}: {result}", nameof(SolvePart1), result);
    }

    public async Task SolvePart2()
    {
        var input = await _client.GetInputAsync(2024, 5);
        var groups = input.Split("\n\n");
        var orderingRules = groups.First()
            .Split("\n")
            .Select(x => RulePairRegex().Match(x))
            .Select(x => (int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value)))
            .ToList();

        var result = groups.Last()
            .Split("\n")
            .Select(x => x.Split(",").Select(int.Parse).ToList())
            .Where(x => EnsureValidOrder(orderingRules, x) is false)
            .Select(x =>
            {
                //LMAO
                while (EnsureValidOrder(orderingRules, x) is false){}
                return x;
            })
            .Select(x => x.ElementAt(x.Count / 2))
            .Sum();

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
