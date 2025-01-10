namespace BumboApp.Helpers;

/*
 * This helper class is used to calculate the required break time for a shift.
 * Updated on jan 2025
 */
public static class BreakCalculationHelper
{
    public static TimeSpan CalculateRequiredBreak(DateTime start, DateTime end)
    {
        var shiftDuration = end - start;
        return TimeSpan.FromMinutes((int)(shiftDuration.TotalHours / 4.5) * 30);
    }
}