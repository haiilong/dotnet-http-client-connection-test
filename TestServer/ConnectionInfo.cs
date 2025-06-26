namespace TestServer;

public record ConnectionInfo
{
    public int RequestCount { get; set; } = 1;
    public required string? RemoteEndpoint { get; init; }
}