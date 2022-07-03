using Netstore.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Domain.Entities.Products;

public class Color : Entity
{
    public int ColorId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<Product>? Products { get; set; }
    public bool Enabled { get; set; }
}