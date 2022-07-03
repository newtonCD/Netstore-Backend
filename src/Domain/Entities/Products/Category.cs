using Netstore.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Domain.Entities.Products;

public class Category : Entity
{
    public int CategoryId { get; set; }
    public int? ParentId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Product>? Products { get; set; }
    public bool Enabled { get; set; }
}
