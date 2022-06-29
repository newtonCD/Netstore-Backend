using System;
using System.Diagnostics.CodeAnalysis;

namespace Template.Domain.Entities.Base;

[ExcludeFromCodeCoverage]
public abstract class Entity : EntityBase<int>
{
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
}
