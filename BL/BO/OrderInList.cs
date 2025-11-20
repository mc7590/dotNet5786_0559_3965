namespace BO;

public class OrderInList
{
    ///<param name="CourierId">ID of the courier assigned to the order</param>
    ///<param name="OrderId">ID of the order</param>
    ///<param name="OrderType">Type of the order</param>
    ///<param name="AerialDistance">Aerial distance from Burgeranch to the delivery address</param>
    ///<param name="OrderStatus">Current status of the order</param>
    ///<param name="ScheduleStatus">Current schedule status of the order</param>
    ///<param name="RemainingTime">Remaining time to deliver the order</param>
    ///<param name="TotalDeliveryTime">Total time taken for delivery</param>
    ///<param name="TotalDeliveries">Total number of deliveries made for the order</param>
    
    public int? CourierId { get; init; }
    public int OrderId { get; init; }
    public EnumOrderType OrderType { get; init; }
    public double AerialDistance { get; init; }
    public EnumOrderStatus OrderStatus { get; init; }
    public EnumScheduleStatus ScheduleStatus { get; init; }
    public TimeSpan RemainingTime { get; init; }
    public TimeSpan TotalDeliveryTime { get; init; } 
    public int TotalDeliveries { get; init; }

}
