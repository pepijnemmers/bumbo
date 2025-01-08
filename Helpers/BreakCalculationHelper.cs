namespace BumboApp.Helpers;

/*
 * This helper class is used to calculate the required break time for a shift.
 * The required break time is based on the legal requirements in the Netherlands
 * https://www.rijksoverheid.nl/onderwerpen/werktijden/vraag-en-antwoord/wettelijke-regels-pauzes-tijdens-werk
 * Updated on jan 2025
 */
public static class BreakCalculationHelper
{
    public static TimeSpan CalculateRequiredBreak(DateTime start, DateTime end)
    {
        var shiftDuration = end - start;
        return shiftDuration switch
        {
            _ when shiftDuration > TimeSpan.FromHours(5.5) && shiftDuration < TimeSpan.FromHours(10) => TimeSpan.FromMinutes(30),
            _ when shiftDuration >= TimeSpan.FromHours(10) => TimeSpan.FromMinutes(45),
            _ => TimeSpan.Zero
        };
    }
}