///Config.cs
namespace Dal;


internal static class Config
{
    internal const int startOrderId = 1000;
    private static int nextOrderId = startOrderId;
    internal static int NextOrderId { get => nextOrderId++; }

    internal const int startDeliveryId = 1000;
    private static int nextDeliveryId = startDeliveryId;
    internal static int NextDeliveryId { get => nextDeliveryId++; }
    internal static DateTime Clock { get; set; } = DateTime.Now;
    internal static int ManagerId { get; set; } = 0;
    internal static string ManagerPassword { get; set; } = "BDI846924?/";

    internal static string? CompanyAddress { get; set; } = null;
    internal static double? Latitude { get; set; } = null;
    internal static double? Longitude {  get; set; } = null;
    internal static double? MaxDeliveryDistance {  get; set; } = null;  

    internal static double AveCarSpeedKmH { get; set; } = 60;
    internal static double AveMotorcycleSpeedKmH { get; set; } = 60;
    internal static double AveBicycleSpeedKmH { get; set; } = 10;
    internal static double AveWalkingSpeedKmH { get; set; } = 10;

    internal static TimeSpan GetMaxDeliveryTime = TimeSpan.FromHours(2);
    internal static TimeSpan RiskRange = TimeSpan.FromHours(1.5);
    internal static TimeSpan InactivityThreshold  = TimeSpan.FromDays(30);
    
    internal static void Reset()
    {
        nextOrderId = 1000;
        nextDeliveryId = 1000;
        Clock = DateTime.Now;
        ManagerId = 0;
        ManagerPassword = "BDI846924?/";
        CompanyAddress = null;
        Latitude = null;
        Longitude = null;
        MaxDeliveryDistance = null;
        AveCarSpeedKmH = 60;
        AveMotorcycleSpeedKmH = 60;
        AveBicycleSpeedKmH = 15;
        AveWalkingSpeedKmH = 15;
        GetMaxDeliveryTime = TimeSpan.FromHours(2);    
        RiskRange = TimeSpan.FromHours(1.5);
        InactivityThreshold = TimeSpan.FromDays(30);
    }

}
