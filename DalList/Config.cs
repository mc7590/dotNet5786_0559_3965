using System.Runtime.CompilerServices;

///Config.cs
namespace Dal;


internal static class Config
{
    internal const int startOrderId = 1000;
    private static int nextOrderId = startOrderId;

    internal static int NextOrderId 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => nextOrderId++; 
    }

    internal const int startDeliveryId = 10000;
    private static int nextDeliveryId = startDeliveryId;
    internal static int NextDeliveryId 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => nextDeliveryId++;
    }
    internal static DateTime Clock
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set;
    } = DateTime.Now;

    internal static int ManagerId 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set;
    } = 0;

    internal static string ManagerPassword 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = "Aaa12345!";

    internal static string? CompanyAddress 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = null;

    internal static double? Latitude 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = 32.072059916717016;

    internal static double? Longitude 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = 34.82851580681851;

    internal static double? MaxDeliveryDistance 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = null;  

    internal static double AveCarSpeedKmH 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = 60;

    internal static double AveMotorcycleSpeedKmH 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; } = 60;
    internal static double AveBicycleSpeedKmH 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = 10;

    internal static double AveWalkingSpeedKmH 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get;
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set; 
    } = 10;

    internal static TimeSpan GetMaxDeliveryTime = TimeSpan.FromHours(2);
    internal static TimeSpan RiskRange = TimeSpan.FromHours(1.5);
    internal static TimeSpan InactivityThreshold  = TimeSpan.FromDays(30);

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    internal static void Reset()
    {
        nextOrderId = 1000;
        nextDeliveryId = 10000;
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
