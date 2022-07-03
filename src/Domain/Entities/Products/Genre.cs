using Netstore.Domain.Entities.Base;
using System.Collections.Generic;

namespace Netstore.Domain.Entities.Products;

public class Genre : Entity
{
    public int GenreId { get; set; }
    public string? Name { get; set; }
    public string? AlternativeName { get; set; }
    public ICollection<Product>? Products { get; set; }
    public bool Enabled { get; set; }
}
