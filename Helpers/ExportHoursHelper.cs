using BumboApp.Models;

namespace BumboApp.Helpers
{
    public class ExportHoursHelper
    {
        //Calculate the average bonus of a workedHour
        public static double CalculateBonusPercent(WorkedHour workedHour)
        {
            var intervals = GetWorkedIntervals(workedHour);
            var bonusPeriods = ExportHoursHelper.GetBonusPeriods(workedHour.DateOnly.DayOfWeek);

            double totalWorkedTime = 0;
            double totalWeightedBonus = 0;
            foreach (var interval in intervals)
            {
                totalWorkedTime += (interval.End - interval.Start).TotalSeconds;
                foreach (var bonusPeriod in bonusPeriods)
                {
                    totalWeightedBonus += CalculateWeightedTime(interval, bonusPeriod);
                }
            }
            double bonus = ((totalWeightedBonus / totalWorkedTime) - 1) * 100;
            return Math.Round(bonus, 1);
        }

        //Splits a workedHour into a list of intervals based on the breaks
        private static List<(TimeOnly Start, TimeOnly End)> GetWorkedIntervals(WorkedHour workedHour)
        {
            List<Break> breaks = workedHour.Breaks ?? new();
            var intervals = new List<(TimeOnly Start, TimeOnly End)>();
            TimeOnly currentStart = workedHour.StartTime;
            foreach (var brk in breaks.OrderBy(b => b.StartTime))
            {
                if (brk != null && brk.EndTime.HasValue)
                {
                    intervals.Add((currentStart, brk.StartTime));
                    currentStart = brk.EndTime.Value;
                }
            }

            // Add the remaining time after the last break
            if (currentStart < workedHour.EndTime)
            {
                intervals.Add((currentStart, workedHour.EndTime.Value));
            }

            return intervals;
        }

        //calculate the amount of seconds*factor of a period of work and a bonus period
        private static double CalculateWeightedTime((TimeOnly Start, TimeOnly End) period, BonusPeriod bonusPeriod)
        {
            if (period.Start >= bonusPeriod.EndTime || period.End <= bonusPeriod.StartTime)
            {
                return 0;
            }
            var overlapStart = Math.Max(period.Start.ToTimeSpan().TotalSeconds, bonusPeriod.StartTime.ToTimeSpan().TotalSeconds);
            var overlapEnd = Math.Min(period.End.ToTimeSpan().TotalSeconds, bonusPeriod.EndTime.ToTimeSpan().TotalSeconds);

            return (overlapEnd - overlapStart) * bonusPeriod.Multiplier;
        }

        public static List<BonusPeriod> GetBonusPeriods(DayOfWeek dayNumber)
        {
            var result = new List<BonusPeriod>();
            switch (dayNumber)
            {
                case DayOfWeek.Sunday:
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(0, 0),
                        EndTime = new TimeOnly(23, 59),
                        Multiplier = 2d
                    });
                    break;
                case DayOfWeek.Saturday:
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(0, 0),
                        EndTime = new TimeOnly(6, 00),
                        Multiplier = 1.5d
                    });
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(6, 0),
                        EndTime = new TimeOnly(18, 00),
                        Multiplier = 1d
                    });
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(18, 0),
                        EndTime = new TimeOnly(23, 59),
                        Multiplier = 1.5d
                    });
                    break;
                default:
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(0, 0),
                        EndTime = new TimeOnly(6, 00),
                        Multiplier = 1.5d
                    });
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(6, 0),
                        EndTime = new TimeOnly(20, 00),
                        Multiplier = 1d
                    });
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(20, 0),
                        EndTime = new TimeOnly(21, 0),
                        Multiplier = 4d / 3d
                    });
                    result.Add(new BonusPeriod
                    {
                        StartTime = new TimeOnly(21, 0),
                        EndTime = new TimeOnly(23, 59),
                        Multiplier = 1.5d
                    });
                    break;
            }
            return result;
        }
    }
}
