using Netstore.Core.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Core.Domain.Entities.Customers;

public class Group : AuditableEntity
{
    public Group(string name, string description, decimal discount, bool enabled)
    {
        Name = name;
        Description = description;
        Discount = discount;
        Enabled = enabled;
        CreatedBy = string.Empty;
        LastModifiedBy = string.Empty;
    }

    private Group() { }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Discount { get; private set; }
    public bool Enabled { get; private set; }

#pragma warning disable S125 // Sections of code should not be commented out

    // public virtual ICollection<Customer> Customers { get; private set; }
#pragma warning restore S125 // Sections of code should not be commented out
}
