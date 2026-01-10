namespace DO;

/// <summary>
/// Used in Courier entity: eumCourierType enum represents the type of courier.
/// </summary>
public enum EnumDeliveryMethod
{
    Car, Motorcycle, Bicycle, Foot
};


/// <summary>
/// Used in Order entity: EumOrderType represents the type of an order. 
/// </summary>
public enum EnumOrderType
{
    Regular, Express, Overnight
};

/// <summary>
/// Used in Delivery entity: EnumEndDeliveryStatus represents the final status of a delivery process.
/// </summary>
/// <remarks>This enumeration provides various outcomes for a delivery attempt, indicating whether it was
/// successful, refused, canceled, or failed due to other reasons.</remarks>
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
/// Enum for main menu  
/// </summary>
public enum MainMenuOption
{
    Exit = 0,
    ManageCouriers = 1,
    ManageOrders = 2,
    ManageDeliveries = 3,
    ManageConfig = 4,
    InitializeData = 5,
    ResetAll = 6
}

/// <summary>
/// Enum for courier menu
/// </summary>
public enum CourierMenuOption
{
    Exit = 0,
    AddCourier = 1,
    GetCourier = 2,
    GetAllCouriers = 3,
    UpdateCourier = 4,
    DeleteCourier = 5,
    DeleteAllCouriers = 6,

}

/// <summary>
/// Enum for order menu
/// </summary>
public enum OrderMenuOption
{
    Exit = 0,
    AddOrder = 1,
    GetOrder = 2,
    GetAllOrders = 3,
    UpdateOrder = 4,
    DeleteOrder = 5,
    DeleteAllOrders = 6
}

/// <summary>
/// Enum for delivery menu
/// </summary>
public enum DeliveryMenuOption
{
    Exit = 0,
    AddDelivery = 1,
    GetDelivery = 2,
    GetAllDeliveries = 3,
    UpdateDelivery = 4,
    DeleteDelivery = 5,
    DeleteAllDeliveries = 6
}

/// <summary>
/// Enum for config menu
/// </summary>
public enum ConfigMenuOption
{
    Exit = 0,
    Add1MinToClock = 1,
    Add1HourToClock = 2,
    Add1DayToClock = 3,
    Add1WeekToClock = 4,
    Add1MonthToClock = 5,
    ShowCurrentClock = 6,
    SetConfigParameters = 7,
    GetConfigParameters = 8,
    ResetConfigToDefault = 9
}

/// <summary>
/// Enum for set config options
/// </summary>
public enum SetConfigParametersOption
{
    Back = 0,
    SetClock = 1,
    SetCompanyAddress = 2,
    SetLatitude = 3,
    SetLongitude = 4,
    SetMaxDeliveryDistance = 5,
}

/// <summary>
/// Enum for get config options
/// </summary>
public enum GetConfigParametersOption
{
    Back = 0,
    GetClock = 1,
    GetCompanyAddress = 2,
    GetLatitude = 3,
    GetLongitude = 4,
    GetMaxDeliveryDistance = 5,
    GetMaxDeliveryTime = 6,
    GetRiskRange = 7,
    GetInactivityThreshold = 8
}

