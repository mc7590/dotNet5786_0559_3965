namespace BO;

public class Order
{
    ///<param name="Id">ID of the order</param>
    ///<param name="OrderType">Type of the order</param>
    ///<param name="Description">Description of the order</param>
    ///<param name="Address">Delivery full address for the order</param>
    ///<param name="Latitude">Latitude coordinate of the delivery address</param>
    ///<param name="Longitude">Longitude coordinate of the delivery address</param>
    ///<param name="AerialDistance">Aerial distance from Burgeranch to the delivery address</param>
    ///<param name="CustomerName">Name of the customer</param>
    ///<param name="CustomerPhone">Phone number of the customer</param>
    ///<param name="Weight">Weight of the order in kilograms</param>
    ///<param name="Fragile">Indicates if the order is fragile</param>
    ///<param name="CreationTime">Time when the order was created</param>
    ///<param name="ExpectedDeliveryTime">Expected delivery time for the order</param>
    ///<param name="MaxDeliveryTime">Max delivery time for the order</param>
    ///<param name="OrderStatus">Current status of the order</param>
    ///<param name="ScheduleStatus">Current schedule status of the order</param>
    ///<param name="RemainingTime">Remaining time to deliver the order</param>
    ///<param name="OrderDelivHist">List of delivery history entries for the order</param>


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
    public IEnumerable<DeliveryPerOrderInList>? OrderDelivHist { get; init; }


}
