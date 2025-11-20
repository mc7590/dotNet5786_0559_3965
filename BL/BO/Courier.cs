/// Module Courier.cs

namespace BO;
/// <summary>
/// Courier Entity represents a courier with all its props
/// </summary>
/// <param name="Id">Personal unique ID of the courier (as in national id card)</param>
/// <param name="Name">The name of the courier</param>
/// <param name="CourierPhone">The cell phone number of the courier</param>
/// <param name="Email">The email address of the courier</param>
/// <param name="Password">The private password of the courier</param>
/// <param name="Active">whether the courier is active </param>
/// <param name="DeliveryMethod">Delivery method: car, motorcycle, bicycle, or on foot</param>
/// <param name="StartedWorking">The time when the courier started to work</param>
/// <param name="MaxPersonalDistance">Max aerial distance from company to order address the courier agrees to deliver</param>
/// <param name="TotalOnTimeDeliveries">The total of orders delivered on time</param>
/// <param name="TotalLateDeliveries">The total of orders delivered late</param>
/// <param name="ActiveDeliveryOrder">The current active delivery order assigned to the courier (if any)</param>
public class Courier
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? CourierPhone { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool Active { get; set; }
    public EnumDeliveryMethod DeliveryMethod { get; set; }
    public DateTime StartedWorking { get; init; }
    public double? MaxPersonalDistance { get; set; }
    public int TotalOnTimeDeliveries { get; init; }
    public int TotalLateDeliveries { get; init; }
    public OrderInProgress? ActiveDeliveryOrder { get; set; }
}
