using Netstore.Core.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Core.Domain.Entities.Products;

public class Grid : Entity
{
    public int GridId { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
    public bool Enabled { get; set; }
}
