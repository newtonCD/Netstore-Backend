using Netstore.Domain.Entities.Base;
using Netstore.Domain.Entities.Customers;

namespace Netstore.Domain.Entities.Products;

public class Rating : Entity
{
    public int RatingId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int Value { get; set; }
}
