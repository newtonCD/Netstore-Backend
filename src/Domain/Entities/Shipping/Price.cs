using Netstore.Domain.Entities.Base;
using System;

namespace Netstore.Domain.Entities.Shipping;
public class Price : Entity
{
    public long Id { get; set; }
    public int ServiceID { get; set; }
    public Service? Service { get; set; }
    public string? ZipCodeStart { get; set; }
    public string? ZipCodeEnd { get; set; }
    public int WeightStart { get; set; }
    public int WeightEnd { get; set; }
    public decimal BasePrice { get; set; }
    public decimal ExtraWeightPrice { get; set; }
    public decimal Factor { get; set; }
    public decimal NotificationReceiptPrice { get; set; }
    public decimal OwnHandsPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public string? HomeDelivery { get; set; }
    public string? WeekendDelivery { get; set; }
    public int DeliveryTime { get; set; }
    public string? Country { get; set; }
    public DateTime PriceDate { get; set; }
}
