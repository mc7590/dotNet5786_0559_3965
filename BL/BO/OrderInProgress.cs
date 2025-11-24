//models OrderInProgress.cs
namespace BO;
/// <summary>
/// OrderInProgress Entity represents an order that is currently being delivered by a courier
/// </summary>
/// <param name="DeliveryId">The unique ID of the delivery</param>
/// <param name="OrderId">The unique ID of the order</param>
/// <param name="OrderType">The type of the order (Regular, Express, Overnight)</param>
/// <param name="Description">The description of the order</param>
/// <param name="Address">The delivery address of the order</param>
/// <param name="AerialDistance">The aerial distance from the company to the delivery address</param>
/// <param name="ActualDistance">The actual distance traveled to deliver the order (if available)</param>
/// <param name="CustomerName">The name of the customer</param>
/// <param name="CustomerPhone">The phone number of the customer</param>
/// <param name="ExpectedDeliveryTime">The expected delivery time of the order</param>
/// <param name="MaxDeliveryTime">The maximum delivery time of the order</param>
/// <param name="OrderStatus">The current status of the order</param>
/// <param name="ScheduleStatus">The schedule status of the delivery (OnTime, InRisk, Late)</param>
/// <param name="RemainingTime">The remaining time to deliver the order, The time difference between the order’s maximum delivery time and the current system time.</param>
public class OrderInProgress
{
    public int DeliveryId { get; init; }
    public int OrderId { get; init; }
    public EnumOrderType OrderType { get; init; }
    public string? Description { get; init; }
    public string? Address { get; init; }
    public double AerialDistance { get; init; }
    public double? ActualDistance { get; init; }
    public string? CustomerName { get; init; }
    public string? CustomerPhone { get; init; }
    public DateTime ExpectedDeliveryTime { get; init; }
    public DateTime MaxDeliveryTime { get; init; }
    public EnumOrderStatus OrderStatus { get; init; }
    public EnumScheduleStatus ScheduleStatus { get; init; }
    public TimeSpan RemainingTime { get; init; }
}
