//Model OpenOrderInList.cs
namespace BO;
/// <summary>
/// OpenOrderInList Entity represents a summary of a opem delivery
/// </summary>
/// <param name="CourierId">The unique ID of the courier</param>
/// <param name="OrderId">The unique ID of the order</param>
/// <param name="OrderType">The type of the order (Regular, Express, Overnight)</param>
/// <param name="Weight">The weight of the order (if applicable)</param>
/// <param name="Fragile">Indicates if the order is fragile (if applicable)</param>
/// <param name="Adrress">The delivery address of the order</param>
/// <param name="AerialDistance">The aerial distance from the company to the delivery address</param>
/// <param name="DistanceInKm">The estimated distance to deliver the order in kilometers (if available)</param>
/// <param name="EstimatedArrivalTime">The estimated arrival time for the order delivery</param>
/// <param name="ScheduleStatus">The schedule status of the delivery (OnTime, InRisk, Late)</param>
/// <param name="RemainingTime">The remaining time to deliver the order, The time difference between the order’s maximum delivery time and the current system time.</param>
/// <param name="MaxDeliveryTime">The maximum delivery time of the order</param>
public class OpenOrderInList
{
    public int CourierId { get; init; }
    public int OrderId { get; init; }
    public EnumOrderType OrderType { get; init; }
    public double? Weight { get; init; }
    public bool? Fragile { get; init; }
    public string? Adrress { get; init; }
    public double AerialDistance { get; init; }
    public double? DistanceInKm { get; init; }
    public TimeSpan EstimatedArrivalTime { get; init; }
    public EnumScheduleStatus ScheduleStatus { get; init; }
    public TimeSpan RemainingTime { get; init; }
    public DateTime MaxDeliveryTime { get; init; }

}
