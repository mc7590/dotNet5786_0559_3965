namespace BO;

public class Order
{
    public int Id { get; init; }
    public EnumOrderType OrderType { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double AerialDistance { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public double? Weight { get; set; }
    public bool? Fragile { get; set; }
    public DateTime CreationTime { get; init; }
    public DateTime? ExpectedDeliveryTime { get; init; }
    public DateTime? MaxDeliveryTime { get; init; }
    public EnumOrderStatus OrderStatus { get; init; }
    public EnumScheduleStatus ScheduleStatus { get; init; }
    public TimeSpan RemainingTime { get; init; }
    public List<DeliveryPerOrderInList>? OrderDelivHist { get; init; }



}
