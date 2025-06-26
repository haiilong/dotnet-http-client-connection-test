namespace TestClient;

public record TestResponse
{
    public required string ConnectionId { get; init; }
    public required int RequestNumber { get; init; }

    public override string ToString()
    {
        return $" Connection {ConnectionId[^3..]}, {GetOrdinalSuffix(RequestNumber)} Request";
    }

    private static string GetOrdinalSuffix(int number)
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