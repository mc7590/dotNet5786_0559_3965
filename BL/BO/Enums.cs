global using EnumCourierFieldFilter = BO.EnumDeliveryMethod;

namespace BO;


/// <summary>
/// DeliveryMethod represents the method of delivery used by the courier.
/// </summary>
public enum EnumDeliveryMethod
{
    None,
    Car,
    Motorcycle, 
    Bicycle, 
    Foot
};

/// <summary>
/// EumOrderType represents the type of an order. 
/// </summary>
public enum EnumOrderType
{
    None,
    Regular,
    Express,
    Overnight
};

/// <summary>
/// OrderStatus represents the status of the order
/// </summary>
public enum EnumOrderStatus
{
    None,
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
    Unknown,
    Delivered,
    RefusedToReceive,
    Canceled,
    CustomerNotFound,
    Failed
}

/// <summary>
/// Specifies the fields by which courier data can be sorted.
/// </summary>
public enum EnumCourierFieldSort
{
    None,
    Id,
    Name,
    StartedWorking,
    TotalOnTimeDeliveries,
    TotalLateDeliveries
}

/// <summary>
/// Specifies the fields by which order data can be filtered
/// </summary>
public enum EnumOrderFieldFilter
{
//CourierId,
//OrderId,
OrderType,
//AerialDistance,
OrderStatus //,
//ScheduleStatus,
//RemainingTime,
//TotalDeliveryTime,
//TotalDeliveries
}

/// <summary>
/// Specifies the fields by which order data can be sorted
/// </summary>
public enum EnumOrderFieldSort
{
    None,
    CourierId,
    OrderId,
    OrderType,
    AerialDistance,
    OrderStatus,
    ScheduleStatus,
    RemainingTime,
    TotalDeliveryTime,
    TotalDeliveries
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

public enum EnumOpenOrderInListField
{
    CourierId,
    OrderId,
    OrderType,
    Address,
    DistanceInKm,
    RemainingTime,
}

/// <summary>
/// represents the fields by which close delivery data can be filtered
/// </summary>
public enum EnumClosedDeliveryInListField
{
    DeliveryId,
    OrderId,
    OrderType,
    Address,
    DeliveryMethod,
    DistanceInKm,
    TotalDeliveryTime
}

/// <summary>
/// represents the fields by which couriers data can be filtered
/// </summary>
public enum EnumActiveCourier
{
    None,
    Active,
    Inactive
}


public enum MainMenuOption
{
    Exit = 0,
    CourierFunctions = 1,
    OrderFunctions = 2,
    ConfigFunctions = 3
}

public enum CourierMenuOption
{
    Exit = 0,   
    LogIn = 1, //should be forced to log in before using courier functions
    AddCourier = 2,
    ShowCourierById = 3,
    ShowListOfCouriers = 4,
    UpdateCourier = 5,
    DeleteCourier = 6,
    NumberOfDeliveriesOnTimeForCourier = 7,
    NumberOfDeliveriesLateForCourier = 8,
    AssignDeliveryToCourier = 9,
    CloseDeliveriesForCourier = 10
}

public enum OrderMenuOptions
{
    Exit = 0,
    AddOrder = 1,
    ShowOrderById = 2,
    ShowListOfOrders = 3,
    UpdateOrder = 4,
    DeleteOrder = 5,
    CancelOrder = 6,
    AmountOfOrderByStatus = 7,
    EndOrderStatus = 8,
    CreateDeliveryForOrder = 9,
    ClosedDeliveriesInListToCourier=10,
    ListOfOpenOrderToChoose = 11

}

public enum ConfigMenuOptions
{
    Exit = 0,
    MoveClock = 1,
    GetClock = 2,
    GetConfig = 3,
    InitializeDB = 4,
    ResetDB = 5,
    SetConfig = 6
}