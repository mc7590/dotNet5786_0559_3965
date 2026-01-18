using System.Runtime.CompilerServices;
using Infrastructure;

namespace Dal;

internal static class Config
{
    internal const string s_data_config_xml = "data-config.xml";
    internal const string s_couriers_xml = "couriers.xml";
    internal const string s_orders_xml = "orders.xml";
    internal const string s_deliveries_xml = "deliveries.xml";



    internal static int NextOrderId
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextOrderId");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextOrderId", value);
    }
    internal static int NextDeliveryId
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextDeliveryId");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextDeliveryId", value);
    }

    internal static DateTime Clock
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDateVal(s_data_config_xml, "Clock");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDateVal(s_data_config_xml, "Clock", value);
    }

    internal static int ManagerId
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigIntVal(s_data_config_xml, "ManagerId");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigIntVal(s_data_config_xml, "ManagerId", value);
    }
    
    internal static string ManagerPassword 
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigStringVal(s_data_config_xml, "ManagerPassword");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigStringVal(s_data_config_xml, "ManagerPassword", value);
    }

    internal static string? CompanyAddress
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigStringVal(s_data_config_xml, "CompanyAddress");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigStringVal(s_data_config_xml, "CompanyAddress", value ?? "");
    }
    internal static double? Latitude
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "Latitude");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "Latitude", value ?? 0);
    }
    internal static double? Longitude
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "Longitude");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "Longitude", value ?? 0);
    }
    internal static double? MaxDeliveryDistance
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "MaxDeliveryDistance");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "MaxDeliveryDistance", value ?? 3);
    }

    internal static double AveCarSpeedKmH
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveCarSpeedKmH");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveCarSpeedKmH", value);
    }
    internal static double AveMotorcycleSpeedKmH
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveMotorcycleSpeedKmH");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveMotorcycleSpeedKmH", value);  
    }
    internal static double AveBicycleSpeedKmH
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveBicycleSpeedKmH");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveBicycleSpeedKmH", value);
    }
    internal static double AveWalkingSpeedKmH
    {
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveWalkingSpeedKmH");
        [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveWalkingSpeedKmH", value);
    }

    internal static TimeSpan GetMaxDeliveryTime = TimeSpan.FromHours(2);
    internal static TimeSpan RiskRange = TimeSpan.FromHours(1.5);
    internal static TimeSpan InactivityThreshold = TimeSpan.FromDays(30);

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    internal static void Reset()
    {
        NextOrderId = 1000;
        NextDeliveryId = 10000;
        Clock = DateTime.Now;
        ManagerId = 0;
        ManagerPassword = "Aaa12345!";
        CompanyAddress = null;
        Latitude = Infrastructure.GlobalConstants.Coordinates.Company_Lat;
        Longitude = Infrastructure.GlobalConstants.Coordinates.Company_Lon;
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
