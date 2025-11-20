namespace BO;

public class OrderInProgress
{
    public int DeliveryId { get; init; }
    public int OrderId { get; init; }
    public EnumOrderType OrderType { get; init; }
    public string? Description { get; init; }
    public string? Address { get; init; }
    public double AirDistance { get; init; }
    public double? ActualDistance { get; init; }
    public string? CustomerName { get; init; }
    public string? CustomerPhone { get; init; }
    public DateTime ExpectedDeliveryTime { get; init; }
    public DateTime MaxDeliveryTime { get; init; }
    public EnumOrderStatus OrderStatus { get; init; }
    public EnumScheduleStatus ScheduleStatus { get; init; }
    public TimeSpan RemainingTime { get; init; }
}
