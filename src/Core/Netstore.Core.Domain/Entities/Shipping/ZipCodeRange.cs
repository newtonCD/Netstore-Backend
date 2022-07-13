using Netstore.Domain.Entities.Base;

namespace Netstore.Domain.Entities.Shipping;

public class ZipCodeRange : Entity
{
    public int Id { get; set; }
    public string ServiceCode { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string ValidInRange { get; set; }
    public bool IsProcessed { get; set; }
}
