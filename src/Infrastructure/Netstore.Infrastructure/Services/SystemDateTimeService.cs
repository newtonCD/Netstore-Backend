using Netstore.Core.Application.Interfaces.Services;
using System;

namespace Netstore.Infrastructure.Services;

public class SystemDateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}