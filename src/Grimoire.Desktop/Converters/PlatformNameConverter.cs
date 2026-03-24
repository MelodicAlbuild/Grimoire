using System.Globalization;
using Avalonia.Data.Converters;
using Grimoire.Shared.Enums;

namespace Grimoire.Desktop.Converters;

public class PlatformNameConverter : IValueConverter
{
    public static readonly PlatformNameConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PlatformType platform)
        {
            return platform switch
            {
                PlatformType.NintendoSwitch => "Switch",
                PlatformType.NintendoDS => "DS",
                PlatformType.Nintendo3DS => "3DS",
                PlatformType.GameBoy => "GB/GBA",
                _ => platform.ToString()
            };
        }
        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
