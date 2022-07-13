using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Netstore.Core.Domain.Entities.Base;

public abstract class EntityBase
{
    private readonly List<DomainEventBase> _domainEvents = new();

    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    internal void ClearDomainEvents() => _domainEvents.Clear();

    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
}
