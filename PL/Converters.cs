using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL;
/// <summary>
/// Converts "Update" string to boolean true (for IsReadOnly property).
/// </summary>
public class ConvertModeToReadOnly : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string buttonText && buttonText == "Update")
        {
            return true;
        }

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts "Update" string to Visibility.Visible. Otherwise Collapsed.
/// </summary>
public class ConvertModeToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string buttonText && buttonText == "Update")
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}