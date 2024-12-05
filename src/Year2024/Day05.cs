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

    private static bool EnsureValidPagesOrder(List<(int before, int after)> orderingRules, List<int> pages)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            var currentPage = pages.ElementAt(i);
            var dependentRules = orderingRules.Where(rule => rule.after == currentPage);
            foreach (var (beforePage, _) in dependentRules)
            {
                int expectedBeforeIndex = pages.IndexOf(beforePage);
                if (expectedBeforeIndex > i)
                {
                    var swapPage = pages[expectedBeforeIndex];
                    pages[expectedBeforeIndex] = currentPage;
                    pages[i] = swapPage;
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
            .Select(rule => (int.Parse(rule.Groups[1].Value), int.Parse(rule.Groups[2].Value)))
            .ToList();

        var result = groups.Last()
            .Split("\n")
            .Select(x => x.Split(",").Select(int.Parse).ToList())
            .Where(pages => EnsureValidPagesOrder(orderingRules, pages))
            .Sum(pages => pages.ElementAt(pages.Count / 2));

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
            .Where(pages => EnsureValidPagesOrder(orderingRules, pages) is false)
            .Select(pages =>
            {
                //LMAO
                while (EnsureValidPagesOrder(orderingRules, pages) is false) { }
                return pages;
            })
            .Sum(x => x.ElementAt(x.Count / 2));

        _logger.LogInformation("{part}: {result}", nameof(SolvePart2), result);
    }
}
