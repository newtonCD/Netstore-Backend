using Netstore.Core.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Core.Domain.Entities.Customers;

public class Group : Entity
{
    public int GroupId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Discount { get; set; }
    public virtual ICollection<Customer> Customers { get; set; }
    public bool Enabled { get; set; }
}