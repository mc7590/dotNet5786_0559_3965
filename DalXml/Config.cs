namespace Dal;

internal static class Config
{
    internal const string s_data_config_xml = "data-config.xml";
    internal const string s_couriers_xml = "couriers.xml";
    internal const string s_orders_xml = "orders.xml";
    internal const string s_deliveries_xml = "deliveries.xml";

    internal static int NextOrderId
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextOrderId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextOrderId", value);
    }
    internal static int NextDeliveryId
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextDeliveryId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextDeliveryId", value);
    }

    internal static DateTime Clock
    {
        get => XMLTools.GetConfigDateVal(s_data_config_xml, "Clock");
        set => XMLTools.SetConfigDateVal(s_data_config_xml, "Clock", value);
    }

    internal static int ManagerId
    {
        get => XMLTools.GetConfigIntVal(s_data_config_xml, "ManagerId");
        set => XMLTools.SetConfigIntVal(s_data_config_xml, "ManagerId", value);
    }
    
    internal static string ManagerPassword 
    {
        get => XMLTools.GetConfigStringVal(s_data_config_xml, "ManagerPassword");
        set => XMLTools.SetConfigStringVal(s_data_config_xml, "ManagerPassword", value);
    }

    internal static string? CompanyAddress
    {
        get => XMLTools.GetConfigStringVal(s_data_config_xml, "CompanyAddress");
        set => XMLTools.SetConfigStringVal(s_data_config_xml, "CompanyAddress", value ?? "");
    }
    internal static double? Latitude
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "Latitude");
        set=> XMLTools.SetConfigDoubleVal(s_data_config_xml, "Latitude", value ?? 0);
    }
    internal static double? Longitude
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "Longitude");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "Longitude", value ?? 0);
    }
    internal static double? MaxDeliveryDistance
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "MaxDeliveryDistance");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "MaxDeliveryDistance", value ?? 3);
    }

    internal static double AveCarSpeedKmH
    {
        get=> XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveCarSpeedKmH");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveCarSpeedKmH", value);
    }
    internal static double AveMotorcycleSpeedKmH
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveMotorcycleSpeedKmH");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveMotorcycleSpeedKmH", value);  
    }
    internal static double AveBicycleSpeedKmH
    {
        get=> XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveBicycleSpeedKmH");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveBicycleSpeedKmH", value);
    }
    internal static double AveWalkingSpeedKmH
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "AveWalkingSpeedKmH");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "AveWalkingSpeedKmH", value);
    }

    internal static TimeSpan GetMaxDeliveryTime = TimeSpan.FromHours(2);
    internal static TimeSpan RiskRange = TimeSpan.FromHours(1.5);
    internal static TimeSpan InactivityThreshold = TimeSpan.FromDays(30);
    

    internal static void Reset()
    {
        NextOrderId = 1000;
        NextDeliveryId = 1000;
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
