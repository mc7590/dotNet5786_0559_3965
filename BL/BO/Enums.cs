namespace BO;

/// <summary>
/// Used in Courier entity: eumCourierType enum represents the type of courier.
/// </summary>
public enum EnumDeliveryMethod
{
    Car, Motorcycle, Bicycle, Foot
};

/// <summary>
/// Used in OrderInProgress entity: EumOrderType represents the type of an order. 
/// </summary>
public enum EnumOrderType
{
    Regular, Express, Overnight
};

/// <summary>
/// Used in OrderInProgress entity: OrderStatus represents the status of the order
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
/// 
/// </summary>
public enum EnumScheduleStatus
{
    OnTime,
    InRisk,
    Late
};
