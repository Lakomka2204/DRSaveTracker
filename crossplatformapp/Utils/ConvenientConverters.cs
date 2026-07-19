
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using crossplatformapp.Utils.Startup;
using DRSTCore;

namespace crossplatformapp.Utils;

public class TimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan time)
        {
            if (time == TimeSpan.Zero)
                return "--:--";
            return $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
        else 
            return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return "ConvertBack not implemented sorry";
    }
}

public class NullToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return "ConvertBack not implemented sorry";
    }

}
public class ChapterColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int chapter)
        {
            switch(chapter)
            {
                case 1:
                    return Brushes.CornflowerBlue;
                case 2:
                    return Brushes.LightGreen;
                case 3:
                    return Brushes.IndianRed;
                case 4:
                    return Brushes.SlateBlue;
                case 5:
                    return Brushes.Gold;
            }
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class BoolToYesNoConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return !!b ? "Yes" : "Np";
        else
            return "I don't know, maybe hello world";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
public class LatestBackupColorConverter : IMultiValueConverter
{
    private static readonly ChapterColorConverter ChapterConverter = new();
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 2)
            throw new ArgumentException("WE NEED TWO OF THEM");
        if (values.Any(v => v == null))
            return AvaloniaProperty.UnsetValue;
        if (values[0] is not SaveFileInfo save)
            return AvaloniaProperty.UnsetValue;
        if (values[1] is not SaveFileInfo backup)
            return AvaloniaProperty.UnsetValue;

        bool isLast = save.LastWrite == backup.LastWrite;
        
        return isLast ? ChapterConverter.Convert(
            backup.Chapter,
            targetType,
            parameter,
            culture
        ) : AvaloniaProperty.UnsetValue;
    }

}