using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Netstore.API.Extensions;
using Netstore.Infrastructure.Extensions;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Host.Configure();
    builder.Services.Configure(builder.Configuration);

    WebApplication app = builder.Build();
    app.UseInfrastructure(builder.Configuration);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server shutting down...");
    Log.CloseAndFlush();
}

#pragma warning disable S3903 // Types should be defined in named namespaces
#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable S1118 // Utility classes should not have public constructors
[ExcludeFromCodeCoverage]
public partial class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
#pragma warning restore CA1050 // Declare types in namespaces
#pragma warning restore S3903 // Types should be defined in named namespaces
{
}