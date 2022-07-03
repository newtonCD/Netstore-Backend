using Netstore.Domain.Entities.Base;

namespace Netstore.Domain.Entities.Products;

public class ProductVariation : Entity
{
    public int ProductVariationId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int ColorId { get; set; }
    public Color? Color { get; set; }
    public int SizeId { get; set; }
    public Size? Size { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool Enabled { get; set; }
}
