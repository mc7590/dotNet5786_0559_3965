// Module ClosedDeliveryInList.cs
namespace BO;
/// <summary>
/// ClosedDeliveryInList Entity represents a summary of a completed delivery
/// </summary>
/// <param name="DeliveryId">The unique ID of the delivery</param>
/// <param name="OrderId">The unique ID of the order</param>
/// <param name="OrderType">The type of the order (Regular, Express, Overnight)</param>
/// <param name="Address">The delivery address of the order</param>
/// <param name="DeliveryMethod">The method of delivery used by the courier (Car, Motorcycle, Bicycle, Foot)</param>
/// <param name="DistanceInKm">The total distance traveled to deliver the order in kilometers</param>
/// <param name-="TotalDeliveryTime">The total time taken to complete the delivery</param>
/// <param name="EndDeliveryStatus">The final status of the delivery process (Delivered, RefusedToReceive, Canceled, CustomerNotFound, Failed)</param>"
public class ClosedDeliveryInList
{
    public int DeliveryId { get; init; }
    public int OrderId { get; init; }
    public EnumOrderType OrderType { get; init; }
    public string? Address { get; init; }
    public EnumDeliveryMethod DeliveryMethod { get; init; }
    public double DistanceInKm { get; init; }
    public TimeSpan TotalDeliveryTime { get; init; }
    public EnumEndDeliveryStatus EndDeliveryStatus { get; init; }

}
