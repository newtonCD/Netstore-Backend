using System;

namespace Netstore.Core.Application.Enums;

/// <summary>
/// Bit field enumerator CoreServices
/// </summary>
[Flags]
public enum CoreServices
{
    None = 0,
    All = 1,
    AutoMapper = 2,
    Cache = 4,
    Cors = 8,
    FluentValidation = 16,
    HealthChecks = 32,
    IpRateLimit = 64,
    JsonWebToken = 128,
    MediatR = 256,
    OpenTelemetry = 512,
    Polly = 1024,
    Serilog = 2048,
    Swagger = 4096
}