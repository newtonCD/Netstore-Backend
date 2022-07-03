using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Netstore.Domain;

[ExcludeFromCodeCoverage]
public abstract class DomainEventBase : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
