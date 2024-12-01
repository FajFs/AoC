namespace AoC.Clients;

public class AdventOfCodeClient(
    ILogger<AdventOfCodeClient> _logger,
    FileSystemCache _fileSystemCache,
    HttpClient _httpClient)
{
    private readonly ILogger<AdventOfCodeClient> _logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly FileSystemCache _fileSystemCache = _fileSystemCache ?? throw new ArgumentNullException(nameof(_fileSystemCache));
    private readonly HttpClient _httpClient = _httpClient ?? throw new ArgumentNullException(nameof(_httpClient));

    private int _year;
    private int _day;

    private string _cacheKey => $"../../../Inputs/year{_year}day{_day}.txt";
    private Uri _relativeUri => new($"{_year}/day/{_day}/input", UriKind.Relative);

    public async Task<string> GetInputAsync(int year, int day, bool forceFreshInput = false)
    {
        _year = year;
        _day = day;

        var input = forceFreshInput
            ? await FetchInputFromAdventOfCode()
            : await _fileSystemCache.LookupAsync(_cacheKey) ?? await FetchInputFromAdventOfCode();

        return input ?? throw new Exception($"Failed to fetch input for year {_year} day {_day}");
    }

    private async Task<string?> FetchInputFromAdventOfCode()
    {
        var response = await _httpClient.GetAsync(_relativeUri);
        if (response.IsSuccessStatusCode is false)
        {
            _logger.LogError("Failed to fetch input from Advent of Code. Status code: {statusCode} Reason: {reason}", response.StatusCode, response.ReasonPhrase);
            Environment.Exit(1);
        }

        var content = await response.Content.ReadAsStringAsync();

        //trim last line if it's empty
        if (content.EndsWith("\n"))
            content = content[..^1];

        await _fileSystemCache.PutAsync(_cacheKey, content);
        return content;
    }
}
