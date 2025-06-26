using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using TestServer;

var builder = WebApplication.CreateBuilder(args);

var connectionTracker = new ConnectionTracker();
builder.Services.AddSingleton(connectionTracker);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5000, listenOptions =>
    {
        listenOptions.UseConnectionLogging();
    });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    var connectionId = context.Connection.Id;
    var remoteEndpoint = context.Connection.RemoteIpAddress + ":" + context.Connection.RemotePort;
    
    Console.WriteLine(context.Request.Protocol);
    connectionTracker.RecordRequest(connectionId, remoteEndpoint);
    
    await next();
});

app.MapGet("/api/test", (ConnectionTracker tracker, HttpContext context) =>
{
    var connectionId = context.Connection.Id;
    var info = tracker.GetConnectionInfo(connectionId);
    
    return new
    {
        ConnectionId = connectionId,
        RequestNumber = info.RequestCount
    };
});

app.Run();