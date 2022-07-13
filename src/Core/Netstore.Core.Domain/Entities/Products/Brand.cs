using Netstore.Core.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Core.Domain.Entities.Products;

public class Brand : Entity
{
    public int BrandId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<Product> Products { get; set; }
    public bool Enabled { get; set; }
}
