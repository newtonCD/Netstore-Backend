using Netstore.Core.Domain.Entities.Base;

namespace Netstore.Core.Domain.Entities.Shipping;

public class WeightRange : Entity
{
    public int Id { get; set; }
    public string ServiceCode { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
}
