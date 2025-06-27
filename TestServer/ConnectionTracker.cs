using System.Collections.Concurrent;
using TestServer.Utils;

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
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Connection {connectionId[^3..]}, {OrdinalHelper.GetOrdinalSuffix(info.RequestCount)} Request from {remoteEndpoint}");
    }

    public ConnectionInfo GetConnectionInfo(string connectionId)
    {
        return _connections[connectionId];
    }
}