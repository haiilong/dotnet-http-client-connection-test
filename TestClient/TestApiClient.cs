using System.Net;
using System.Text.Json;

namespace TestClient;

public class TestApiClient
{
    private readonly HttpClient _httpClient;

    public TestApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5000");
    }

    public async Task<TestResponse> GetTestDataAsync()
    {
        var response = await _httpClient.GetStringAsync("/api/test");
        return JsonSerializer.Deserialize<TestResponse>(response,  new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
    }
}