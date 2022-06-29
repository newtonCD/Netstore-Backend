using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Template.Domain;

[ExcludeFromCodeCoverage]
public abstract class DomainEventBase : INotification
{
  public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}