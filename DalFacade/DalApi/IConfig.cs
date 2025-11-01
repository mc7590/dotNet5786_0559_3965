//IConfig.cs
namespace DalApi;

public interface IConfig
{
    /// <summary>
    /// System clock
    /// </summary>
    DateTime Clock { get; set; }

    /// <summary>
    /// company address
    /// </summary>
    string? CompanyAddress { get; set; }

    /// <summary>
    /// Coordinates
    /// </summary>
    double? Latitude { get; set; }
    double? Longitude { get; set; }

    /// <summary>
    /// max delivery distance
    /// </summary>
    double? MaxDeliveryDistance { get; set; }

    /// <summary>
    /// max delivery time
    /// </summary>
    TimeSpan GetMaxDeliveryTime { get; set; }

    /// <summary>
    /// time range for risk deliveries
    /// </summary>
    TimeSpan RiskRange { get; set; }

    /// <summary>
    /// max inactivity time for couriers
    /// </summary>
    TimeSpan InactivityThreshold { get; set; }

    /// <summary>
    /// Resets all configuration data to the initial values.
    /// </summary>
    void Reset();
}
