// Module Delivery.cs
namespace DO;
/// <summary>
/// Delivery Entity represents a Delivery with all its props
/// </summary>

///<param name="CourierId">ID of the courier assigned to the delivery</param>
///<param name="OrderId">ID of the order being delivered</param>
/// <param name="DeliveryMethod">Delivery method: car, motorcycle, bicycle, or on foot</param>
/// <param name="DeliveryStartTime">Date and time when the delivery started</param> 
/// <param name="DistanceInKm">Optional. Actual distance in kilometers from the company to the order address</param>
/// <param name="EndDeliveryStatus">Optional. Final status of the delivery process</param>
/// <param name="EndDeliveryTime">Optional. Date and time when the delivery ended</param>
public record Delivery
(
    int Id,
    int OrderId,
    int CourierId,
    EnumDeliveryMethod DeliveryMethod,
    DateTime DeliveryStartTime,
    double? DistanceInKm=null,
    EnumEndDeliveryStatus? EndDeliveryStatus=null,
    DateTime? EndDeliveryTime= null
)
{
    /// <summary>
    /// Default constructor for stage 3
    /// </summary>
    Delivery() : this(0, 0, 0, EnumDeliveryMethod.Car, DateTime.Now) { }
}
