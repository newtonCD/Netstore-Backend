using Netstore.Domain.Entities.Base;

namespace Netstore.Domain.Entities.Products;
public class Picture : Entity
{
    public int PictureId { get; set; }
    public string PictureUrl { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
