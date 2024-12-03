using BumboApp.Models;

namespace BumboApp.Helpers
{
    public static class LeaveHoursCalculationHelper
    {
        private const int HoursPerDay = 8;

        public static Dictionary<int, int> CalculateLeaveHoursByYearForAllRequests(
                List<LeaveRequest> leaveRequests)
        {
            var totalLeaveHoursByYear = new Dictionary<int, int>();

            foreach (var leaveRequest in leaveRequests)
            {
                // Calculate leave hours by year for the leave request
                var leaveHoursByYear = CalculateLeaveHoursByYear(leaveRequest.StartDate, leaveRequest.EndDate);

                // Put the leave hours into the total dictionary
                foreach (var entry in leaveHoursByYear)
                {
                    if (!totalLeaveHoursByYear.ContainsKey(entry.Key))
                    {
                        totalLeaveHoursByYear[entry.Key] = 0;
                    }

                    totalLeaveHoursByYear[entry.Key] += entry.Value;
                }
            }

            return totalLeaveHoursByYear;
        }

        public static Dictionary<int, int> CalculateLeaveHoursByYear(DateTime start, DateTime end)
        {
            var leaveHoursByYear = new Dictionary<int, int>();

            if (start > end)
                return leaveHoursByYear;

            // Iterate over each year involved in the leave request
            var yearsInvolved = Enumerable.Range(start.Year, end.Year - start.Year + 1);

            foreach (var year in yearsInvolved)
            {
                var yearStart = new DateTime(year, 1, 1);
                var yearEnd = new DateTime(year, 12, 31, 23, 59, 59);

                // Calculate the overlap for the current year
                var effectiveStart = (start > yearStart) ? start : yearStart;
                var effectiveEnd = (end < yearEnd) ? end : yearEnd;

                // Skip if there's no overlap
                if (effectiveStart > effectiveEnd)
                    continue;

                TimeSpan difference = effectiveEnd - effectiveStart;
                int totalDays = difference.Days;

                int amountOfLeaveHours;

                // Calculate leave hours based on the given logic
                if (totalDays > 0)
                {
                    amountOfLeaveHours = totalDays * HoursPerDay;
                }
                else
                {
                    // Handle partial days
                    if (difference.Hours > HoursPerDay)
                    {
                        amountOfLeaveHours = HoursPerDay;
                    }
                    else
                    {
                        amountOfLeaveHours = difference.Hours;
                    }
                }

                // Add the leave hours for the current year to the dictionary
                if (!leaveHoursByYear.ContainsKey(year))
                {
                    leaveHoursByYear[year] = 0;
                }
                leaveHoursByYear[year] += amountOfLeaveHours;
            }
            return leaveHoursByYear;
        }
    }
}
