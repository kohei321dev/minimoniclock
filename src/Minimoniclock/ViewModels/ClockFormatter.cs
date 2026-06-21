using System.Globalization;

namespace Minimoniclock.ViewModels;

public static class ClockFormatter
{
    public static string Format(DateTime value)
    {
        return value.ToString("HH:mm", CultureInfo.InvariantCulture);
    }
}
