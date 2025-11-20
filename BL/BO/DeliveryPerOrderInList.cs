namespace BO;

public class DeliveryPerOrderInList
{
    ///<param name="DeliveryId">ID of the delivery</param>
    ///<param name="CourierId">ID of the courier</param>
    ///<param name="CourierName">Name of the courier</param>
    ///<param name="DeliveryMethod">Method of delivery used</param>
    ///<param name="DelCreationTime">Delivery creation time</param>
    ///<param name="EndDeliveryStatus">Final status of the delivery</param>
    ///<param name="EndDeliveryTime">Time when the delivery ended</param>
    
    public int DeliveryId { get; init; }
    public int CourierId { get; init; }
    public string? CourierName { get; init; }
    public EnumDeliveryMethod DeliveryMethod { get; init; }
    public DateTime DelCreationTime { get; init; }
    public EnumEndDeliveryStatus? EndDeliveryStatus { get; init; }
    public DateTime? EndDeliveryTime { get; init; }


}
