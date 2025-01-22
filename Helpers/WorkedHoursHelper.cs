using BumboApp.Models;
using BumboApp.ViewModels;

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

            if (plannedStart != startTime || plannedEnd != endTime)
            {
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

            var span = actualWorkedDuration - plannedDuration;
            if (span != null)
            {
                span = TimeSpan.FromMinutes(Math.Floor(((TimeSpan)span).TotalMinutes));
            }
            return span;
        }

        public List<WorkedHourViewModel> GetCombinedHours(List<Shift> plannedShifts, List<WorkedHour> workedHours)
        {
            var combinedHours = plannedShifts
            .Select(shift => new
            {
                Shift = shift,
                WorkedHour = workedHours.FirstOrDefault(wh =>
                (shift.Employee != null && wh.Employee.EmployeeNumber == shift.Employee.EmployeeNumber) &&
                wh.DateOnly == DateOnly.FromDateTime(shift.Start))
            })
            .Select(x =>
            {
                // Calculate breaks duration 
                var breakDuration = x.WorkedHour?.Breaks
                    ?.Where(b => b.EndTime != null)
                    .Sum(b => (b.EndTime - b.StartTime)?.Ticks ?? 0) is long ticks && ticks > 0
                    ? TimeSpan.FromTicks(ticks)
                    : TimeSpan.Zero;

                // Calculate total worked time 
                var totalWorkedTime = (x.WorkedHour?.StartTime != null && x.WorkedHour?.EndTime != null)
                    ? x.WorkedHour.EndTime - x.WorkedHour.StartTime - breakDuration
                    : null;

                // Determine if there's a difference
                TimeOnly? plannedStart = TimeOnly.Parse(x.Shift.Start.ToString("HH:mm"));
                TimeOnly? plannedEnd = TimeOnly.Parse(x.Shift.End.ToString("HH:mm"));
                bool hasHourDifference = HasHourDifference(x.WorkedHour?.StartTime, x.WorkedHour?.EndTime, plannedStart, plannedEnd, breakDuration);
                var hourDifference = HourDifference(x.WorkedHour?.StartTime, x.WorkedHour?.EndTime, plannedStart, plannedEnd, breakDuration);

                return new WorkedHourViewModel
                {
                    Id = x.WorkedHour?.Id,
                    Employee = x.Shift.Employee,
                    StartTime = x.WorkedHour?.StartTime,
                    EndTime = x.WorkedHour?.EndTime,
                    Date = DateOnly.FromDateTime(x.Shift.Start),
                    BreaksDuration = breakDuration,
                    TotalWorkedTime = totalWorkedTime,
                    Status = x.WorkedHour?.Status,
                    PlannedShift = x.Shift.Start.ToString("HH:mm") + " - " + x.Shift.End.ToString("HH:mm"),
                    IsFuture = DateOnly.FromDateTime(x.Shift.Start) <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(x.Shift.Start).Month == DateOnly.FromDateTime(DateTime.Now).Month,
                    HasHourDifference = hasHourDifference,
                    HourDifference = hourDifference
                };
            })
            .OrderBy(e => e.Date)
            .ToList();

            return combinedHours;
        }
    }
}
