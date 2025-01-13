using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Helpers
{
    public class WorkedHoursHelper
    {
        public bool HasHourDifference(TimeOnly? startTime, TimeOnly? endTime, TimeOnly? plannedStart, TimeOnly? plannedEnd, TimeSpan? breakDuration)
        {
            if (startTime == null || endTime == null || plannedStart == null || plannedEnd == null)
            {
                // If the worked hours are not available, consider it a difference
                return true;
            }

            // Calculate the planned shift duration
            var plannedDuration = plannedEnd - plannedStart - BreakCalculationHelper.CalculateRequiredBreak(plannedStart.Value, plannedEnd.Value);

            // Calculate the actual worked duration
            var actualWorkedDuration = endTime - startTime - breakDuration;

            // Check if there is a difference
            return actualWorkedDuration != plannedDuration;
        }

        public TimeSpan? HourDifference(TimeOnly? startTime, TimeOnly? endTime, TimeOnly? plannedStart, TimeOnly? plannedEnd, TimeSpan? breakDuration)
        {
            var plannedDuration = plannedEnd - plannedStart - BreakCalculationHelper.CalculateRequiredBreak(plannedStart.Value, plannedEnd.Value);
            var actualWorkedDuration = endTime - startTime - breakDuration;

            return actualWorkedDuration - plannedDuration;
        }
    }
}
