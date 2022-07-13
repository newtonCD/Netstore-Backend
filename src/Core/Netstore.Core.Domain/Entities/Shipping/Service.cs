using Netstore.Domain.Entities.Base;

namespace Netstore.Domain.Entities.Shipping;
public class Service : Entity
{
    public int ServiceId { get; set; }
    public string ServiceCode { get; set; }
    public int ServiceNumber { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long WeightLimit { get; set; }
    public long AdditionalWeightStart { get; set; }
    public int TypeCubingCalculation { get; set; }
    public int AdditionalDeliveryTime { get; set; }
    public decimal AutoInsurance { get; set; }
    public decimal MaxCoverageInsurance { get; set; }
    public long MaxCubicVolume { get; set; }
    public int FactorCubicWeight { get; set; }
    public long CubicExemption { get; set; }
    public long SizeLargestSide { get; set; }
    public long SizeSumSides { get; set; }
    public int MinLength { get; set; }
    public int MinWidth { get; set; }
    public int MinHeight { get; set; }
    public decimal AdValorem { get; set; }
    public decimal MinPurchase { get; set; }
    public bool IsECTservice { get; set; }
    public bool Enabled { get; set; }
}