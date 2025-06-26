using System.Collections.Concurrent;

namespace TestServer;

public class ConnectionTracker
{
    private readonly ConcurrentDictionary<string, ConnectionInfo> _connections = new();

    public void RecordRequest(string connectionId, string? remoteEndpoint)
    {
        _connections.AddOrUpdate(connectionId, 
            new ConnectionInfo { RemoteEndpoint = remoteEndpoint },
            (_, existing) => 
            {
                existing.RequestCount++;
                return existing;
            });

        var info = _connections[connectionId];
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Connection {connectionId[^3..]}, {GetOrdinalSuffix(info.RequestCount)} Request from {remoteEndpoint}");
    }

    public ConnectionInfo GetConnectionInfo(string connectionId)
    {
        return _connections[connectionId];
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