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

## Key Improvements Made

### Code Quality Enhancements
- **Eliminated duplicate code**: Removed duplicate ordinal suffix logic between projects
- **Centralized constants**: Created shared configuration for URLs and JSON serialization
- **Proper resource disposal**: Added `using` statements for ServiceProvider instances
- **Consistent package versions**: Aligned all packages to .NET 8.0 versions
- **Base class extraction**: Created TestScenarioBase to reduce code duplication

### Architecture Improvements
- **Separation of concerns**: Moved utility functions to dedicated helper classes
- **Configuration management**: Centralized server configuration
- **Consistent error handling**: Improved null handling and type safety

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

## Technical Notes

- Uses ASP.NET Core minimal APIs for the server
- Leverages HttpClientFactory for proper HttpClient management
- Demonstrates proper disposal patterns for dependency injection containers
- Shows the difference between PooledConnectionLifetime and HandlerLifetime settings
