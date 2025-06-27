namespace TestServer.Utils;

public static class OrdinalHelper
{
    public static string GetOrdinalSuffix(int number)
    {
        return (number % 100) switch
        {
            11 or 12 or 13 => $"{number}th",
            _ => (number % 10) switch
            {
                1 => $"{number}st",
                2 => $"{number}nd",
                3 => $"{number}rd",
                _ => $"{number}th",
            }
        };
    }
}
