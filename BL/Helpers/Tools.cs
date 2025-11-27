using System.Collections;
using System.Reflection;
using System.Text;
namespace Helpers;

internal static class Tools
{
    /// <summary>
    /// Helper method to convert any object of type T to a string representation of its properties and their values.
    /// </summary>
    public static string ToStringProperty<T>(this T t)
    {
        if (t == null)
        {
            return "NULL";
        }

        // use 'StringBuilder' to efficiently build the output string
        var sb = new StringBuilder();

        // get all properties of T
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        sb.AppendLine($"--- Type: {typeof(T).Name} ---");

        foreach (var prop in properties)
        {
            var value = prop.GetValue(t);

            //check if the property is an collection (List, Array, ICollection etc.) that is not a string
            if (value is IEnumerable enumerable && !(value is string))
            {
                sb.AppendLine($"  {prop.Name}: [");

                //string the collection items
                foreach (var item in enumerable)
                {
                    sb.AppendLine($"    {item?.ToString()}");
                }

                sb.AppendLine("  ]");
            }
            else
            {
                // property is not a collection
                sb.AppendLine($"  {prop.Name}: {value}");
            }
        }

        sb.AppendLine("--------------------------");
        return sb.ToString();
    }
    public static void IsManager(int id)
    {
        if (id != AdminManager.GetConfig().ManagerId)
            throw new UnauthorizedAccessException("Access denied: User is not a manager.");
    }
    public static void IsManagerOrCourier(int id, int courierId)
    {
        if (id != AdminManager.GetConfig().ManagerId && id != courierId)
            throw new UnauthorizedAccessException("Access denied: User is not a manager or courier.");
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
        if (phone[0] != 0 || phone.Length != 10 || !phone.All(char.IsDigit))
            throw new BO.BlInvalidInputException($"Phone number must be 9 or 10 digits");
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

    public static double CalculateDistanceInKm(double longitude, double latitude)
    {
        return 0.0;
    }
    public static double CalculateAerialDistance(double longitude, double latitude)
    {
        return 0.0;
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
}
