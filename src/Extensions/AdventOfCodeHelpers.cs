namespace AoC.Extensions;
public static partial class AocHelpers
{
    public static int AbsolutValue(int x)
        => x < 0 ? -x : x;

    [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    public static partial Regex MatchDigit();

}