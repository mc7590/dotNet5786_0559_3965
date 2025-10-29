/// Module Courier.cs

namespace DO;
/// <summary>
/// Courier Entity represents a courier with all its props
/// </summary>
/// <param name="Id">Personal unique ID of the courier (as in national id card)</param>
/// <param name="Name">The name of the courier</param>
/// <param name="CourierPhone">The cell phone number of the courier</param>
/// <param name="Email">The email address of the courier</param>
/// <param name="Password">The private password of the courier</param>
/// <param name="Active">whether the courier is active (default true)</param>
/// <param name="DeliveryMethod">Delivery method: car, motorcycle, bicycle, or on foot</param>
/// <param name="StartedWorking">The time when the courier started to work</param>
/// <param name="MaxPersonalDistance">Max aerial distance from company to order address the courier agrees to deliver</param>
public record Courier
(
    int Id,
    string Name,
    string CourierPhone,
    string Email,
    string Password,
    bool Active,
    EnumDeliveryMethod DeliveryMethod,
    DateTime StartedWorking,
    double? MaxPersonalDistance = 0
)
{
    /// <summary>
    /// Default constructor for stage 3
    /// </summary>
    public Courier() : this(0, "", "", "", "", true, EnumDeliveryMethod.Car, DateTime.Now) {}
}

