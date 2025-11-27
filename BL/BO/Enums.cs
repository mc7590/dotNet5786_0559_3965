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

/// <summary>
/// Specifies the fields by which courier data can be sorted.
/// </summary>
public enum EnumCourierFieldSort
{
    Id,
    Name,
    StartedWorking,
    TotalOnTimeDeliveries,
    TotalLateDeliveries,
    MaxPersonalDistance
}
public enum EnumCourierFieldFilter
{
    Active,
    DeliveryMethod,
    MaxPersonalDistance
}
/// <summary>
/// Specifies the fields by which order data can be sorted
/// </summary>
public enum EnumOrderFieldSort
{
    Id,
    OrderType,
    AerialDistance,
    CustomerName,
    Weight,
    Fragile,
    CreationTime,
    ExpectedDeliveryTime,
    MaxDeliveryTime,
    OrderStatus,
    ScheduleStatus
}

/// <summary> 
/// EnumUserRole represents the role of a user in the system.
/// <summary>
public enum EnumUserRole
{
    Manager,
    Courier
}

/// <summary>
/// EnumTimeUnit represents time units for clock manipulation.
/// </summary>
public enum EnumTimeUnit
{
    Minute,
    Hour,
    Day,
    Month,
    Year
}