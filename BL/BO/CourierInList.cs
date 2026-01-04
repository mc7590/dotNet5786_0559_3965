//Module CourierInList.cs
namespace BO;
/// <summary>
/// CourierInList Entity represents a courier with basic info for listing purposes
/// </summary>
/// <param name="Id">Personal unique ID of the courier (as in national id card)</param>
/// <param name="Name">The name of the courier</param>
/// <param name="Active">whether the courier is active </param>
/// <param name="DeliveryMethod">Delivery method: car, motorcycle, bicycle, or on foot</param>
/// <param name="StartedWorking">The time when the courier started to work</param>
/// <param name="TotalOnTimeDeliveries">The total of orders delivered on time</param>
/// <param name="TotalLateDeliveries">The total of orders delivered late</param>
/// <param name="OrdersInProgressId">The ID of the current active delivery order assigned to the courier (if any)</param>
public class CourierInList
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public bool Active { get; init; }
    public BO.EnumDeliveryMethod DeliveryMethod { get; init; }
    public DateTime StartedWorking { get; init; }
    public int TotalOnTimeDeliveries { get; init; }
    public int TotalLateDeliveries { get; init; }
    public int OrderInProgressId { get; init; }
}
