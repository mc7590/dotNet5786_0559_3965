using System.Collections;
using System.Reflection;
namespace Helpers;
using BO;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

public static class Tools
{
    /// <summary>
    /// Helper method to convert any object of type T to a string representation of its properties and their values.
    /// </summary>
    public static string ToStringProperty<T>(this T t)
    {
        if (t == null)
            return "null";

        Type type = t.GetType();
        PropertyInfo[] properties = type.GetProperties();

        string str = $"{type.Name} {{ ";

        foreach (var prop in properties)
        {
            object? value = prop.GetValue(t);

            // If property value is null
            if (value == null)
            {
                str += $"{prop.Name} = null, ";
                continue;
            }

            Type valueType = value.GetType();

            // If the value is a collection (but not a string)
            if (value is IEnumerable enumerable && valueType != typeof(string))
            {
                str += $"{prop.Name} = [ ";

                foreach (var item in enumerable)
                {
                    if (item == null)
                    {
                        str += "null, ";
                    }
                    else
                    {
                        Type itemType = item.GetType();

                        // Simple types are printed directly
                        if (itemType.IsPrimitive ||
                            itemType.IsEnum ||
                            itemType == typeof(string) ||
                            itemType == typeof(decimal) ||
                            itemType == typeof(DateTime))
                        {
                            str += $"{item}, ";
                        }
                        else
                        {
                            // Complex objects → recursive call
                            str += item.ToStringProperty() + ", ";
                        }
                    }
                }

                str += "], ";
            }

            // If the value type is simple → print directly
            else if (valueType.IsPrimitive ||
                     valueType.IsEnum ||
                     valueType == typeof(string) ||
                     valueType == typeof(decimal) ||
                     valueType == typeof(DateTime))
            {
                str += $"{prop.Name} = {value}, ";
            }

            // Otherwise, it's a complex object → recursive call
            else
            {
                str += $"{prop.Name} = {value.ToStringProperty()}, ";
            }
        }

        str += "}";

        return str;
    }   
    public static void IsManager(int id)
    {
        if (id != AdminManager.GetConfig().ManagerId)
            throw new BlUnauthorizedException("Access denied: User is not a manager.");
    }
    public static void IsManagerOrCourier(int id, int courierId)
    {
        if (id != AdminManager.GetConfig().ManagerId && id != courierId)
            throw new BlUnauthorizedException("Access denied: User is not a manager or courier.");
    }
    public static void IsValidId(int id)
    {
        if (id < 100000000 || id > 999999999)
            throw new BO.BlInvalidInputException($"ID must be 9 digits");
    }
    public static void IsValidName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit))
            throw new BO.BlInvalidInputException($"Invalid name: '{name}'");
    }
    public static void IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new BO.BlInvalidInputException($"Empty phone number");
        if (phone[0] != '0' || phone.Length != 10 || !phone.All(char.IsDigit))
            throw new BO.BlInvalidInputException($"Invalid phone: '{phone}'");
    }
    public static void IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BO.BlInvalidInputException($"Empty email");
        if (!email.Contains("@") || !email.Contains("."))
            throw new BO.BlInvalidInputException($"Email must contain '@' and '.' characters");
    }
    public static void IsValidAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new BO.BlInvalidInputException($"Empty address");
    }

    /// <summary>
    /// Calculates the estimated road distance in kilometers from the company location to the given coordinates.
    /// </summary>
    public static async Task<double> CalculateDistanceInKm(double longitude, double latitude)
    {
        double? originLat = AdminManager.GetConfig().Latitude;
        double? originLon = AdminManager.GetConfig().Longitude;
        if (originLat == null || originLon == null)
        {
            return 0;
        }
        return await GetDrivingDistanceFromApi(originLat.Value, originLon.Value, latitude, longitude);
    }
    private static readonly HttpClient client = new HttpClient();

    //async net function to get actual distance
    //private static double GetDrivingDistanceFromApi(double originLat, double originLon, double destLat, double destLon) //stage 4
    private static async Task<double> GetDrivingDistanceFromApi(double originLat, double originLon, double destLat, double destLon) //stage 7
    {
        try
        {
            string coordinates = $"{originLon},{originLat};{destLon},{destLat}";
            string url = $"http://router.project-osrm.org/route/v1/driving/{coordinates}?overview=false";

// ביצוע הקריאה ב-Thread נפרד למניעת Deadlock
            //var response = client.GetAsync(url).Result; //stage 4
            HttpResponseMessage response = await client.GetAsync(url); //stage 7

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("routes", out JsonElement routes) && routes.GetArrayLength() > 0)
                    {
                        if (routes[0].TryGetProperty("distance", out JsonElement distanceElement))
                        {
                            // המרחק מתקבל במטרים, מחלקים ב-1000 לקבלת ק"מ
                            return distanceElement.GetDouble() / 1000.0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // במקרה של שגיאה (למשל אין אינטרנט), נחזיר 0
            Console.WriteLine($"Error fetching distance: {ex.Message}");
        }

        return 0;
    }


    /// <summary>
    /// Calculates the aerial distance in kilometers between the company location and the given coordinates using the Haversine formula.
    /// </summary>
    public static double CalculateAerialDistance(double longitude, double latitude)
    {
        double? companyLat = AdminManager.GetConfig().Latitude;
        double? companyLon = AdminManager.GetConfig().Longitude;
        if (companyLat == null || companyLon == null)
        {
            return 0; 
        }
        return GetAerialDistance(companyLat.Value, companyLon.Value, latitude, longitude);
    }
    private static double GetAerialDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double rlat1 = Math.PI * lat1 / 180;
        double rlat2 = Math.PI * lat2 / 180;
        double theta = lon1 - lon2;
        double rtheta = Math.PI * theta / 180;

        double dist =
            Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
            Math.Cos(rlat2) * Math.Cos(rtheta);

        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        //return dist * 1.609344;  
        double km = dist * 1.609344;

        return Math.Round(km, 2);
    }
    public static TimeSpan CalculateTimeDifference(DateTime start, DateTime end)
    {
        return end - start;
    }
    public static string HashPassword(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;
        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));
        bool longEnough = password.Length >= 8;
        return hasUpper && hasLower && hasDigit && hasSpecial && longEnough;
    }
    public static bool VerifyPassword(string password, string? encrypted)
    {
        string hashOfInput = HashPassword(password);
        return string.Equals(hashOfInput, encrypted, StringComparison.OrdinalIgnoreCase);
    }
    public static void UpdateManagerPassword(int id, string newPassword)
    {
        IsManager(id);
        if(!IsStrongPassword(newPassword))
            throw new BO.BlInvalidInputException("Password is not strong enough.");
        var config = AdminManager.GetConfig();
        config.ManagerPassword = newPassword;
        AdminManager.SetConfig(config);
    }
    // Get latitude and longitude from address using geocoding API
    internal static async Task<(double lat, double lon)> GetLatiAndLong(string address)
    {
        Tools.IsValidAddress(address);
        string encoded = UrlEncoder.Default.Encode(address);
        string apiKey = "6967b0585e7ac453693044aou6c470b";
        // request URL 
        string url = $"https://geocode.maps.co/search?q={encoded}&api_key={apiKey}";
        using HttpClient client = new HttpClient();
        // 
        HttpResponseMessage response = await client.GetAsync(url);
        // check if the server response is successful
        if (!response.IsSuccessStatusCode)
        {
            throw new BO.BlInvalidInputException($"Could not get latitude and longitude for address: {address}");
        }
        // read the json response content
        string json = await response.Content.ReadAsStringAsync();
        using JsonDocument document = JsonDocument.Parse(json);
        var root = document.RootElement;
        if (root.GetArrayLength() == 0)
        {
            throw new BO.BlInvalidInputException($"Could not get latitude and longitude for address: {address}");
        }
        var firstResult = root[0]; // get the first result
        // parse latitude and longitude
        double lat = double.Parse(firstResult.GetProperty("lat").GetString()!, CultureInfo.InvariantCulture);
        double lon = double.Parse(firstResult.GetProperty("lon").GetString()!, CultureInfo.InvariantCulture);
        // return latitude and longitude as a tuple
        return (lat, lon);

    }
}
