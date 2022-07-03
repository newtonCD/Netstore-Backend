using Netstore.Domain.Entities.Base;
using Netstore.Domain.Entities.Products;

namespace Netstore.Domain.Entities.Orders;

public class Detail : Entity
{
    public long DetailId { get; set; }
    public long OrderId { get; set; }
    public Order? Order { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}