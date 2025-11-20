namespace BO;

/// <summary>
/// DeliveryMethod represents the method of delivery used by the courier.
/// </summary>
public enum EnumDeliveryMethod
{
    Car, Motorcycle, Bicycle, Foot
};

/// <summary>
/// EumOrderType represents the type of an order. 
/// </summary>
public enum EnumOrderType
{
    Regular, Express, Overnight
};

/// <summary>
/// OrderStatus represents the status of the order
/// </summary>
public enum EnumOrderStatus
{
    Open,
    InProgress,
    Delivered,
    CustomerRefused,
    Canceled
};

/// <summary>
/// ScheduleStatus represents the schedule status of a delivery
/// </summary>
public enum EnumScheduleStatus
{
    OnTime,
    InRisk,
    Late
};

/// <summary>
/// EnumEndDeliveryStatus represents the final status of a delivery process.
/// </summary>
public enum EnumEndDeliveryStatus
{
    Delivered, RefusedToReceive, Canceled, CustomerNotFound, Failed
}