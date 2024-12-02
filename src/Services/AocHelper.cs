namespace AoC.Extensions;
public partial class AocHelper
{
    public int AbsolutValue(int x)
        => x < 0 ? -x : x;

    [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    public partial Regex MatchDigit();

}