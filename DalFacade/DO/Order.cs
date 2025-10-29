// Module Order.cs 

namespace DO;
/// <summary>
/// Order Entity represents an order with all its props
/// </summary>

/// <param name="Id">Order ID (as in national id card)</param>
/// <param name="OrderType">Type of the order</param>
/// <param name="Description">Optional. short textual description of the order</param>
/// <param name="Address">Delivery full address</param>
/// <param name="Latitude">Latitude of the address (updated by logic layer)</param>
/// <param name="Longitude">Longitude of the address (updated by logic layer)</param>
/// <param name="CustomerName">Full name of the customer</param>
/// <param name="CustomerPhone">Customer phone number (10 digits starting with 0)</param>
/// <param name="OrderCreationTime">The date and time the order was created</param>
/// <param name="Weight">Optional. weight of the order</param>
/// <param name="Fragile">Optional. Indicates whether the order is fragile</param>
public record Order
(
    int Id,
    EnumOrderType OrderType,
    string? Description,
    string Address,
    double Latitude,
    double Longitude,
    string CustomerName,
    string CustomerPhone,
    DateTime OrderCreationTime,
    double? Weight = null,
    bool? Fragile = null
)
{
    /// <summary>
    /// Default constructor for stage 3
    /// </summary>
    public Order() : this(0, EnumOrderType.Regular, null, "", 0, 0, "", "", DateTime.Now) { }
}
public enum EnumOrderType { Regular, Express, Overnight };

