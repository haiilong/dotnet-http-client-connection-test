using System.Text.Json;
using TestClient.Constants;

namespace TestClient;

public class TestApiClient
{
    private readonly HttpClient _httpClient;

    public TestApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(AppConstants.ServerBaseUrl);
    }

    public async Task<TestResponse> GetTestDataAsync()
    {
        var response = await _httpClient.GetStringAsync(AppConstants.TestEndpoint);
        return JsonSerializer.Deserialize<TestResponse>(response, AppConstants.JsonOptions)!;
    }
}