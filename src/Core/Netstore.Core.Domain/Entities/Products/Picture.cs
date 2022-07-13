using Netstore.Core.Domain.Entities.Base;

namespace Netstore.Core.Domain.Entities.Products;
public class Picture : Entity
{
    public int PictureId { get; set; }
    public string PictureUrl { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
