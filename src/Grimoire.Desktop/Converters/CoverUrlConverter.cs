using System.Globalization;
using Avalonia.Data.Converters;

namespace Grimoire.Desktop.Converters;

/// <summary>
/// Converts a relative cover URL (e.g., "/api/covers/1") to a full absolute URL
/// using the configured server base address, and returns whether a cover exists.
/// </summary>
public class CoverUrlConverter : IValueConverter
{
    public static readonly CoverUrlConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string url && !string.IsNullOrEmpty(url))
        {
            // The HttpClient base address is set in App.axaml.cs
            // Build the full URL for the image source
            var baseUrl = "https://emu.melodicalbuild.com";
            return new Uri(baseUrl + url);
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

/// <summary>
/// Returns true if the string value is not null or empty. Used for cover visibility toggling.
/// </summary>
public class NotNullOrEmptyConverter : IValueConverter
{
    public static readonly NotNullOrEmptyConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string s && !string.IsNullOrEmpty(s);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class IsNullOrEmptyConverter : IValueConverter
{
    public static readonly IsNullOrEmptyConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not string s || string.IsNullOrEmpty(s);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
