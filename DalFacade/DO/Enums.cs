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
    Delivered, RefusedToReceive, Canceled, CustomerNotFound, Failed
}