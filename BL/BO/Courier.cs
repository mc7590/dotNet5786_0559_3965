namespace BO;

public class Courier
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? CourierPhone { get; init; }
    public string? Email { get; init; }
    public string? Password { get; set; }
    public bool Active { get; set; }
    public EnumDeliveryMethod DeliveryMethod { get; set; }
    public DateTime StartedWorking { get; init; }
    public double? MaxPersonalDistance { get; init; }
    public int TotalOnTimeDeliveries { get; set; }
    public int TotalLateDeliveries { get; set; }
    public OrderInProgress? ActiveDeliveryOrder { get; set; }
}
