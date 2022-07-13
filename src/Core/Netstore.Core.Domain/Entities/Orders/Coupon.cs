using Netstore.Core.Domain.Entities.Base;
using System;

namespace Netstore.Core.Domain.Entities.Orders;

public class Coupon : Entity
{
    public int Id { get; set; }
    public string Code { get; set; }
    public decimal Discount { get; set; }
    public int TypeOfDiscount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Active { get; set; }
}
