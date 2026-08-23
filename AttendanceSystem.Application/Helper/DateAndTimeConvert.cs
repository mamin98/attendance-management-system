using System.Globalization;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class DateAndTimeHelperConvert
{
    public static DateOnly GetDateOnly(string? date)
    {
        return date is null
            ? DateOnly.MinValue
            : DateOnly.ParseExact(
                date,
                AttendanceSystemConsts.DateFormat,
                CultureInfo.InvariantCulture);
    }

    public static DateTime GetDateTime(string? date)
    {
        return date is null
            ? DateTime.MinValue
            : DateTime.ParseExact(
                date,
                AttendanceSystemConsts.DateFormat,
                CultureInfo.InvariantCulture);
    }
    public static DateTime GetLocalDataTime(DateTime date) => date.ToLocalTime();    

    public static string ConvertDecimalToTime(decimal value)
    {
        if (value <= 0)
            return "00:00";

        int totalMinutes = (int)Math.Round(value * 60);
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;
        
        return $"{hours:D2}:{minutes:D2}";
    }

    public static DateTime FirstDayOfMonth(this DateTime date) => new(date.Year, date.Month, 1);

    public static DateTime LastDayOfMonth(this DateTime date) =>
        new(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
}

