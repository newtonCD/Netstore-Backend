using Netstore.Domain.Entities.Base;

namespace Netstore.Domain.Entities.Orders;

public class Status : Entity
{
    public int StatusId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}