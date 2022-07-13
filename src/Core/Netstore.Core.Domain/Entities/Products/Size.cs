using Netstore.Core.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Core.Domain.Entities.Products;

public class Size : Entity
{
    public int SizeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Product> Products { get; set; }
    public bool Enabled { get; set; }
}