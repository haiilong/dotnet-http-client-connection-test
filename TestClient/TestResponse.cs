using TestClient.Utils;

namespace TestClient;

public record TestResponse
{
    public required string ConnectionId { get; init; }
    public required int RequestNumber { get; init; }

    public override string ToString()
    {
        return $" Connection {ConnectionId[^3..]}, {OrdinalHelper.GetOrdinalSuffix(RequestNumber)} Request";
    }
}