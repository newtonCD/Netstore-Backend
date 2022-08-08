using System;

namespace Netstore.Core.Application.Interfaces.Services;

public interface IDateTimeService : ITransientService
{
    DateTime NowUtc { get; }
}