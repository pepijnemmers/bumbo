namespace BumboApp.Helpers;

/*
 * This helper class is used to calculate the required break time for a shift.
 * Updated on jan 2025
 */
public static class BreakCalculationHelper
{
    private static TimeSpan CalculateBreak(TimeSpan shiftDuration)
    {
        return TimeSpan.FromMinutes((int)(shiftDuration.TotalHours / 4.5) * 30);
    }

    public static TimeSpan CalculateRequiredBreak(DateTime start, DateTime end)
    {
        return CalculateBreak(end - start);
    }

    public static TimeSpan CalculateRequiredBreak(TimeOnly start, TimeOnly end)
    {
        return CalculateBreak(end - start);
    }
}