namespace AoC.Clients;

public class AdventOfCodeClient(
    HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    private int _year;
    private int _day;

    private string FileName => $"../../../Inputs/year{_year}day{_day}.txt";
    private string Url => $"{_year}/day/{_day}/input";

    public async Task<string> GetInputAsync(int year, int day, bool forceFreshInput = false)
    {
        _year = year;
        _day = day;

        var input = forceFreshInput
            ? await FetchInputFromAdventOfCode()
            : await LocalFileSystemLookup() ?? await FetchInputFromAdventOfCode();

        return input ?? throw new Exception("No data found");
    }

    private async Task<string?> LocalFileSystemLookup()
        => File.Exists(FileName)
        ? await File.ReadAllTextAsync(FileName)
        : null;

    private async Task<string?> FetchInputFromAdventOfCode()
    {
        var response = await _httpClient.GetAsync(Url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        //trim last line if it's empty
        if (content.EndsWith("\n"))
            content = content[..^1];

        await StoreDataInLocalFileSystem(content);
        return content;
    }

    private async Task StoreDataInLocalFileSystem(string content)
        => await File.WriteAllTextAsync(FileName, content);
}

