# .NET HTTP Client Connection Test

This solution demonstrates different HTTP client connection patterns and their behavior regarding connection reuse in .NET 8.

## Projects

### TestServer
A minimal ASP.NET Core Web API that tracks HTTP connections and requests.

**Features:**
- Connection tracking with unique identifiers
- Request counting per connection
- Logging of connection reuse patterns

### TestClient
A console application that tests various HTTP client scenarios.

**Test Scenarios:**
1. **Same HttpClient Instance**: Tests connection reuse with a single HttpClient instance
2. **Different HttpClient Instances**: Tests connection creation with multiple HttpClient instances
3. **Typed Client**: Tests connection reuse using HttpClientFactory with typed clients
4. **Pooled Connection Lifetime**: Tests connection recycling based on PooledConnectionLifetime
5. **Handler Lifetime**: Tests handler disposal and recreation effects on connections

## Running the Tests

1. Start the server:
   ```bash
   cd TestServer
   dotnet run
   ```

2. Run the client tests:
   ```bash
   cd TestClient
   dotnet run
   ```

## Expected Behavior

- **Scenario 1**: Same connection ID across all requests (connection reuse)
- **Scenario 2**: Different connection IDs for each request (new connections)
- **Scenario 3**: Same connection ID across requests (HttpClientFactory reuse)
- **Scenario 4**: Connection recreation every ~3 seconds due to PooledConnectionLifetime
- **Scenario 5**: Connection recreation due to handler disposal despite long PooledConnectionLifetime
