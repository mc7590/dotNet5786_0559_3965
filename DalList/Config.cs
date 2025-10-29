namespace Dal;

static internal class Config
{
    internal static int NextOrderId = 1000;
    internal static int NextDeliveryId = 1000;
    internal static DateTime Clock { get; set; } = DateTime.Now;
    internal static int ManagerId;
    internal static string ManagerPassword = "BDI846924?/";
    internal const string? CompanyAddress = null;
    internal static double? Latitude = null;
    internal static double? Longitude = null;
    internal static double? MaxDeliveryDistance = null;
    internal static double AveCarSpeedKmH;
    internal static double AveMotorcycleSpeedKmH;
    internal static double AveBicycleSpeedKmH;
    internal static double AveWalkingSpeedKmH;
    internal static TimeSpan GetMaxDeliveryTime;
    internal static TimeSpan RiskRange;
    internal static TimeSpan InactivityThreshold;

    internal static void Reset()
    {
        Clock = DateTime.Now;
    }
}
