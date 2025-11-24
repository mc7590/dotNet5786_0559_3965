namespace BO;

public class Config
{
    public int ManagerId { get; set; }
    public string? ManagerPassword { get; set; } 

    public string? CompanyAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; } 
    public double? MaxDeliveryDistance { get; set; } 

    public double AveCarSpeedKmH { get; set; }
    public double AveMotorcycleSpeedKmH { get; set; }
    public double AveBicycleSpeedKmH { get; set; } 
    public double AveWalkingSpeedKmH { get; set; }
    public TimeSpan GetMaxDeliveryTime { get; set; } 
    public TimeSpan RiskRange { get; set; }
    public TimeSpan InactivityThreshold { get; set; }
}
