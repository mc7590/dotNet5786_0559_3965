//ConfigImplementation.cs
using DalApi;

namespace Dal;

/// <summary>
/// Configuration implementation class.
/// </summary>
internal class ConfigImplementation : IConfig
{
    /// <summary>
    /// Get/set the current clock time used for configuration.
    /// </summary>
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock=value; 
    }

    /// <summary>
    /// Get/set the company address.
    /// </summary>
    public string? CompanyAddress 
    { 
        get => Config.CompanyAddress;
        set => Config.CompanyAddress=value;
    }

    /// <summary>
    /// Get/set the latitude coordinate.
    /// </summary>
    public double? Latitude
    {
        get => Config.Latitude;
        set=> Config.Latitude=value;
    }

    /// <summary>
    /// Get/set the longitude coordinate.
    /// </summary>
    public double? Longitude
    {
        get => Config.Longitude;
        set => Config.Longitude = value;
    }

    /// <summary>
    /// Get/set the max delivery distance.
    /// </summary>
    public double? MaxDeliveryDistance
    {
        get => Config.MaxDeliveryDistance;
        set => Config.MaxDeliveryDistance = value;
    }

    /// <summary>
    /// Get/set the max delivery time.
    /// </summary>
    public TimeSpan GetMaxDeliveryTime
    {
        get => Config.GetMaxDeliveryTime;
        set => Config.GetMaxDeliveryTime = value;
    }

    /// <summary>
    /// Get/set the time range for risk deliveries.
    /// </summary>
    public TimeSpan RiskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;
    }

    /// <summary>
    /// Get/set the max inactivity time for couriers.
    /// </summary>
    public TimeSpan InactivityThreshold
    {
        get => Config.InactivityThreshold;
        set => Config.InactivityThreshold = value;
    }

    /// <summary>
    /// Resets all configuration data to the initial values.
    /// </summary>
    public void Reset()
    {
        Config.Reset();
    }
}
