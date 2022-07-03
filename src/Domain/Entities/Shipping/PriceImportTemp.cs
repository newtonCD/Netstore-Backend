using Netstore.Domain.Entities.Base;

namespace Netstore.Domain.Entities.Shipping;

public class PriceImportTemp : Entity
{
    public long Id { get; set; }
    public string? ServiceCode { get; set; }
    public string? ZipCodeStart { get; set; }
    public string? ZipCodeEnd { get; set; }
    public int WeightStart { get; set; }
    public int WeightEnd { get; set; }
    public decimal BasePrice { get; set; }
    public decimal NotificationReceiptPrice { get; set; }
    public decimal OwnHandsPrice { get; set; }
    public string? HomeDelivery { get; set; }
    public string? WeekendDelivery { get; set; }
    public int DeliveryTime { get; set; }
}
