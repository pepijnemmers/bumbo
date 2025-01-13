namespace BumboApp.Helpers;

public static class DateConvertorHelper
{
    public static DateOnly GetMondayOfWeek(DateOnly date)
    {
        var dayOfWeek = (int) date.DayOfWeek;
        var daysToMonday = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
        return date.AddDays(-daysToMonday);
    }
}