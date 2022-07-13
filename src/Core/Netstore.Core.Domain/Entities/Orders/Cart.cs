using Netstore.Domain.Entities.Base;
using Netstore.Domain.Entities.Customers;
using System;

namespace Netstore.Domain.Entities.Orders;

public class Cart : Entity
{
    public long CartId { get; set; }
    public Guid CartCode { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}
