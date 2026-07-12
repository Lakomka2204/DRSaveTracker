
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace crossplatformapp;

public class TimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan time)
        {
            return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
        else 
            return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return "ConvertBack not implemented sorry";
    }
}
