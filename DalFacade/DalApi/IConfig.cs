//IConfig.cs
namespace DalApi;

public interface IConfig
{
    /// <summary>
    /// id and password of the manager
    /// </summary>
    int ManagerId { get; set; }
    string ManagerPassword { get; set; }

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
    /// average speed of the car, motorcycle, bicycle and walk in kph.
    /// </summary>
    double AveCarSpeedKmH { get; set; }
    double AveMotorcycleSpeedKmH { get; set; }
    double AveBicycleSpeedKmH { get; set; }
    double AveWalkingSpeedKmH { get; set; }

    /// <summary>
    /// Resets all configuration data to the initial values.
    /// </summary>
    void Reset();
}
