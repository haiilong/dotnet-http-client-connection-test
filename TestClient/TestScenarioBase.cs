using System.Diagnostics;
using System.Text.Json;
using TestClient.Constants;

namespace TestClient;

public abstract class TestScenarioBase
{
    public static void PrintScenarioHeader(string scenarioName, string description)
    {
        Console.WriteLine($"{scenarioName}: {description}\n");
    }

    public static async Task MakeRequestAndMeasure(HttpClient httpClient, int requestNumber)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await httpClient.GetStringAsync(AppConstants.ServerBaseUrl + AppConstants.TestEndpoint);
        stopwatch.Stop();
        
        var data = JsonSerializer.Deserialize<TestResponse>(response, AppConstants.JsonOptions);
        Console.WriteLine($"  Request {requestNumber}: {data} (took {stopwatch.ElapsedMilliseconds}ms)");
    }

    public static async Task MakeRequestAndMeasure(TestApiClient client, int requestNumber)
    {
        var stopwatch = Stopwatch.StartNew();
        var data = await client.GetTestDataAsync();
        stopwatch.Stop();
        
        Console.WriteLine($"  Request {requestNumber}: {data} (took {stopwatch.ElapsedMilliseconds}ms)");
    }

    public static async Task MakeRequestAndMeasureWithTime(TestApiClient client, int requestNumber)
    {
        var stopwatch = Stopwatch.StartNew();
        var data = await client.GetTestDataAsync();
        stopwatch.Stop();
        
        Console.WriteLine($"  Request {requestNumber}: {data} at {DateTime.Now:HH:mm:ss} (took {stopwatch.ElapsedMilliseconds}ms)");
    }
}
