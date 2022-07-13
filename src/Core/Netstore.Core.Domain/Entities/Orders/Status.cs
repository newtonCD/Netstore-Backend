using Netstore.Core.Domain.Entities.Base;

namespace Netstore.Core.Domain.Entities.Orders;

public class Status : Entity
{
    public int StatusId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}