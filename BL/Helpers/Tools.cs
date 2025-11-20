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

}
