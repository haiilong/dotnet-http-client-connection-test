using Microsoft.Extensions.DependencyInjection;
using TestClient;

Console.WriteLine("HTTP Connection Reuse Test\n");

await TestScenario1_SameHttpClientInstance();
await TestScenario2_DifferentHttpClientInstances();
await TestScenario3_TypedClient();
await TestScenario4_PooledConnectionLifetime();
await TestScenario5_HandlerLifetimeTest();

return;

static async Task TestScenario1_SameHttpClientInstance()
{
    TestScenarioBase.PrintScenarioHeader("Scenario 1", "Same HttpClient instance (should reuse connections)");
    
    using var httpClient = new HttpClient();
    for (var i = 1; i <= 5; i++)
    {
        await TestScenarioBase.MakeRequestAndMeasure(httpClient, i);
    }
    Console.WriteLine();
}

static async Task TestScenario2_DifferentHttpClientInstances()
{
    TestScenarioBase.PrintScenarioHeader("Scenario 2", "Different HttpClient instances (should create new connections)");
    
    for (var i = 1; i <= 5; i++)
    {
        using var httpClient = new HttpClient();
        await TestScenarioBase.MakeRequestAndMeasure(httpClient, i);
    }
    Console.WriteLine();
}

static async Task TestScenario3_TypedClient()
{
    TestScenarioBase.PrintScenarioHeader("Scenario 3", "Using TypedClient with default settings (should reuse connections)");
    
    var services = new ServiceCollection();
    services.AddHttpClient<TestApiClient>();

    await using var serviceProvider = services.BuildServiceProvider();
    
    for (var i = 1; i <= 5; i++)
    {
        var client = serviceProvider.GetRequiredService<TestApiClient>();
        await TestScenarioBase.MakeRequestAndMeasure(client, i);
    }
    Console.WriteLine();
}

static async Task TestScenario4_PooledConnectionLifetime()
{
    TestScenarioBase.PrintScenarioHeader("Scenario 4", "Testing PooledConnectionLifetime of 3 seconds");
    
    var services = new ServiceCollection();
    services.AddHttpClient<TestApiClient>()
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromSeconds(3),
            MaxConnectionsPerServer = 1
        });

    await using var serviceProvider = services.BuildServiceProvider();
    
    Console.WriteLine("  Making requests with 2-second delays (connection should be recreated after every 2 requests):\n");
    
    for (var i = 1; i <= 5; i++)
    {
        var client = serviceProvider.GetRequiredService<TestApiClient>();
        await TestScenarioBase.MakeRequestAndMeasureWithTime(client, i);
        
        if (i < 5) await Task.Delay(2000);
    }
    
    Console.WriteLine();
}

static async Task TestScenario5_HandlerLifetimeTest()
{
    TestScenarioBase.PrintScenarioHeader("Scenario 5", "Testing HandlerLifeTime of 1 seconds with long PooledConnectionLifeTime");
    
    var services = new ServiceCollection();
    services.AddHttpClient<TestApiClient>()
        .SetHandlerLifetime(TimeSpan.FromSeconds(1))
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            MaxConnectionsPerServer = 1
        });;


    await using var serviceProvider = services.BuildServiceProvider();
    
    Console.WriteLine("  Making requests with 2-second delays (connection should be recreated):\n");
    
    for (var i = 1; i <= 5; i++)
    {
        var client = serviceProvider.GetRequiredService<TestApiClient>();
        await TestScenarioBase.MakeRequestAndMeasureWithTime(client, i);
        
        if (i < 5) await Task.Delay(2000);
    }
    
    Console.WriteLine();
}