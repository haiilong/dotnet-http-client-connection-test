using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using TestClient;

Console.WriteLine("HTTP Connection Reuse Test\n");

// await TestHttpVersion();

await TestScenario1_SameHttpClientInstance();
await TestScenario2_DifferentHttpClientInstances();
await TestScenario3_TypedClient();
await TestScenario4_PooledConnectionLifetime();
await TestScenario5_HandlerLifetimeTest();

return;


static async Task TestScenario1_SameHttpClientInstance()
{
    Console.WriteLine("Scenario 1: Same HttpClient instance (should reuse connections)\n");
    
    using var httpClient = new HttpClient();
    for (var i = 1; i <= 5; i++)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await httpClient.GetStringAsync("http://localhost:5000/api/test");
        stopwatch.Stop();
        
        var data = JsonSerializer.Deserialize<TestResponse>(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Console.WriteLine($"  Request {i}: {data} (took {stopwatch.ElapsedMilliseconds}ms)");
    }
    Console.WriteLine();
}

static async Task TestScenario2_DifferentHttpClientInstances()
{
    Console.WriteLine("Scenario 2: Different HttpClient instances (should create new connections)\n");
    
    for (var i = 1; i <= 5; i++)
    {
        using var httpClient = new HttpClient();
        var stopwatch = Stopwatch.StartNew();
        var response = await httpClient.GetStringAsync("http://localhost:5000/api/test");
        stopwatch.Stop();
        
        var data = JsonSerializer.Deserialize<TestResponse>(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Console.WriteLine($"  Request {i}: {data} (took {stopwatch.ElapsedMilliseconds}ms)");
    }
    Console.WriteLine();
}

static async Task TestScenario3_TypedClient()
{
    Console.WriteLine("Scenario 3: Using TypedClient with default settings (should reuse connections)");
    
    var services = new ServiceCollection();
    services.AddHttpClient<TestApiClient>();
    
    var serviceProvider = services.BuildServiceProvider();
    
    for (var i = 1; i <= 5; i++)
    {
        var client = serviceProvider.GetRequiredService<TestApiClient>();
        var stopwatch = Stopwatch.StartNew();
        var data = await client.GetTestDataAsync();
        stopwatch.Stop();
        
        Console.WriteLine($"  Request {i}: {data} (took {stopwatch.ElapsedMilliseconds}ms)");
    }
    Console.WriteLine();
}

static async Task TestScenario4_PooledConnectionLifetime()
{
    Console.WriteLine("Scenario 4: Testing PooledConnectionLifetime of 3 seconds");
    
    var services = new ServiceCollection();
    services.AddHttpClient<TestApiClient>()
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromSeconds(3),
            MaxConnectionsPerServer = 1
        });
    
    var serviceProvider = services.BuildServiceProvider();
    
    Console.WriteLine("  Making requests with 2-second delays (connection should be recreated after every 2 requests):\n");
    
    for (var i = 1; i <= 5; i++)
    {
        var client = serviceProvider.GetRequiredService<TestApiClient>();
        var stopwatch = Stopwatch.StartNew();
        var data = await client.GetTestDataAsync();
        stopwatch.Stop();
        
        Console.WriteLine($"  Request {i}: {data} at {DateTime.Now:HH:mm:ss} (took {stopwatch.ElapsedMilliseconds}ms)");
        
        if (i < 5) await Task.Delay(2000);
    }
    
    Console.WriteLine();
}

static async Task TestScenario5_HandlerLifetimeTest()
{
    Console.WriteLine("Scenario 5: Testing HandlerLifeTime of 1 seconds with long PooledConnectionLifeTime");
    
    var services = new ServiceCollection();
    services.AddHttpClient<TestApiClient>()
        .SetHandlerLifetime(TimeSpan.FromSeconds(1))
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromSeconds(100),
            MaxConnectionsPerServer = 1
        });
    
    var serviceProvider = services.BuildServiceProvider();
    
    Console.WriteLine("  Making requests with 2-second delays (connection should be recreated):\n");
    
    for (var i = 1; i <= 5; i++)
    {
        var client = serviceProvider.GetRequiredService<TestApiClient>();
        var stopwatch = Stopwatch.StartNew();
        var data = await client.GetTestDataAsync();
        stopwatch.Stop();
        
        Console.WriteLine($"  Request {i}: {data} at {DateTime.Now:HH:mm:ss} (took {stopwatch.ElapsedMilliseconds}ms)");
        
        if (i < 5) await Task.Delay(2000);
    }
    
    Console.WriteLine();
}