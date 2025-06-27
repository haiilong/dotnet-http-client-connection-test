using System.Text.Json;

namespace TestClient.Constants;

public static class AppConstants
{
    public const string ServerBaseUrl = "http://localhost:5000";
    public const string TestEndpoint = "/api/test";
    
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
