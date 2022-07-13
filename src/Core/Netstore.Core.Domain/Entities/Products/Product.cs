using Netstore.Domain.Entities.Base;

namespace Netstore.Domain.Entities.Products;

public class Product : Entity
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
    public string ImageThumbnailUrl { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public int GridId { get; set; }
    public Grid Grid { get; set; }
    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public int ProductTypeId { get; set; }
    public ProductType ProductType { get; set; }
    public bool Enabled { get; set; }
}
