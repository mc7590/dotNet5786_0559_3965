using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

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

/// <summary>
/// Converts EnumDeliveryMethod to a specific SolidColorBrush.
/// </summary>
public class ConvertDelMethodToBrush : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.EnumDeliveryMethod method)
        {
            return method switch
            {
                BO.EnumDeliveryMethod.Foot => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB0B0")),
                BO.EnumDeliveryMethod.Bicycle => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F08B8B")),
                BO.EnumDeliveryMethod.Motorcycle => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9A9A")),
                BO.EnumDeliveryMethod.Car => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6F6F")),
                _ => Brushes.Transparent
            };
        }
        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); 
    }
}

/// <summary>
/// Converts EnumOrderStatus to a specific SolidColorBrush.
/// </summary>
public class ConvertOrderStatusToBrush : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.EnumOrderStatus method)
        {
            return method switch
            {
                BO.EnumOrderStatus.Open => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB0B0")),
                BO.EnumOrderStatus.InProgress => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F08B8B")),
                BO.EnumOrderStatus.Delivered => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9A9A")),
                BO.EnumOrderStatus.CustomerRefused => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6F6F")),
                BO.EnumOrderStatus.Canceled => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F06060")),
                _ => Brushes.Transparent
            };
        }
        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts EnumOrderType to a specific SolidColorBrush.
/// </summary>
public class ConvertOrderTypeToBrush : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.EnumOrderType method)
        {
            return method switch
            {
                BO.EnumOrderType.Regular => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E2E2")),
                BO.EnumOrderType.Express => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9D9D9")),
                BO.EnumOrderType.Overnight => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E0D8")),
                _ => Brushes.Transparent
            };
        }
        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertIsActiveToBrush : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isActive)
        {
            return isActive ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E2E2")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9D9D9"));
        }
        return Brushes.Transparent;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value == null ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b) return !b;
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b) return !b;
        return true;
    }
}

public class CourierEnableConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // to check if courier is active and has no active order
        if (values[0] is bool isActive)
        {
            var activeOrder = values[1];
            return isActive && (activeOrder == null);
        }
        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

}
/// <summary>
/// simulation button content converter from bool to "Start"/"Stop"
/// </summary>
public class BoolToSimStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (bool)value ? "⏹ Stop" : "▶ Start";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// simulation button color converter from bool to Red/Green
/// </summary>
public class BoolToSimColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (bool)value ? Brushes.Red : Brushes.Green;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
