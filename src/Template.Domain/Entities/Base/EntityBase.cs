using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Template.Domain.Entities.Base;

[ExcludeFromCodeCoverage]
public abstract class EntityBase<TId> : IEntityBase<TId>
{
    private readonly List<DomainEventBase> _domainEvents = new();
    public virtual TId Id { get; protected set; }

    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    internal void ClearDomainEvents() => _domainEvents.Clear();

    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
}