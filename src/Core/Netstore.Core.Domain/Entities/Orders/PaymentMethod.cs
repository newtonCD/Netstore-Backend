using Netstore.Domain.Entities.Base;
using Netstore.Domain.Entities.Customers;
using System.Collections.Generic;

namespace Netstore.Domain.Entities.Orders;

public class PaymentMethod : Entity
{
    public int PaymentMethodId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Discount { get; set; }
    public virtual ICollection<Customer> Customers { get; set; }
    public bool Enabled { get; set; }
}
