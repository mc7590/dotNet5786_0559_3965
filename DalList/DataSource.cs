namespace Dal;

static internal class DataSource
{
    /// <summary>
    /// Contains all couriers in the system.
    /// </summary>
    internal static List<DO.Courier> Couriers { get; } = new();

    /// <summary>
    /// Contains all deliverys in the system.
    /// </summary>
    internal static List<DO.Delivery> Deliverys { get; } = new();

    /// <summary>
    /// Contains all orders in the system.
    /// </summary>
    internal static List<DO.Order> Orders { get; } = new();
}
