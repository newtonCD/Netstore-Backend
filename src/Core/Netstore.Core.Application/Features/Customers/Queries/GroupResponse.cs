using System;

namespace Netstore.Core.Application.Features.Customers.Queries;

public class GroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Discount { get; set; }
    public bool Enabled { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public string CreatedBy { get; set; }
    public string LastModifiedBy { get; set; }
}
