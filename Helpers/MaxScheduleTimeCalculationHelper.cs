using BumboApp.Models;

namespace BumboApp.Helpers
{
    public class MaxScheduleTimeCalculationHelper
    {

        private const int MaxShiftLengthAdult = 12;
        private const int MaxWeeklyHoursAdult = 60;

        private const int MaxWeeklyHoursAlmostAdult = 40;
        private const int TimeframeInWeeksMaxWeeklyHoursAlmostAdult = 4;
        private const int MaxHoursWithSchoolAlmostAdult = 9;

        private const int MaxWeeklyHoursUnderSixteen = 40;
        private const int MaxWeeklyHoursSchoolweekUnderSixteen = 12;
        private const int MaxHoursWithSchoolUnderSixteen = 8;
        private const int MaxWorkingDaysUnderSixteen = 5;
        private readonly TimeOnly _maxWorkTimeForUnderSixteen = new TimeOnly(19, 00, 00);

        private const float BreakTimeHours = (float)0.5;
        private readonly int[] _breakTimes = { 4, 8 };
        readonly BumboDbContext _context;

        public MaxScheduleTimeCalculationHelper(BumboDbContext context)
        {
            _context = context;
        }

        public int GetWorkingHours(IEnumerable<Shift> shifts)
        {
            float hours = 0;
            foreach (Shift shift in shifts)
            {
                TimeSpan time = shift.End - shift.Start;
                hours += time.Hours;
                foreach (int breakTime in _breakTimes)
                {
                    if (time.Hours > breakTime)
                    {
                        hours -= BreakTimeHours;
                    }
                }
            }
            return (int)Math.Ceiling(hours);
        }

        public float GetWorkingHoursNoZero(IEnumerable<Shift> shifts)
        {
            float hours = GetWorkingHours(shifts);
            if (hours == 0)
            {
                return 0.1F;
            }
            return hours;
        }
        public int GetMaxTimeContract(Employee employee, DateOnly startDate)
        {
            int hours = employee.ContractHours;
            List<Shift> shifts = employee.Shifts
                .Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date <= startDate.AddDays(6).ToDateTime(new TimeOnly()))
                .ToList();
            if (shifts.Count != 0)
            {
                float calculationHours = hours - GetWorkingHours(shifts);
                foreach (int br in _breakTimes)
                {
                    if (calculationHours >= br)
                    {
                        calculationHours += BreakTimeHours;
                    }
                }
                hours = (int)Math.Ceiling(calculationHours);
            }
            return hours;
        }

        public int GetMaxTimePrognose(Department department, DateOnly scheduleDate)
        {
            Prognosis? prognosis = _context.Prognoses.FirstOrDefault(e => e.Date == scheduleDate && e.Department == department);

            List<Shift> departmentDayShifts = _context.Shifts
            .Where(e => e.Start.Date == scheduleDate.ToDateTime(new TimeOnly()))
            .Where(e => e.Department == department)
            .ToList();

            if (prognosis == null)
            {
                return 0;
            }

            int missingHours = (int)Math.Ceiling(prognosis.NeededHours - GetWorkingHours(departmentDayShifts) - _breakTimes.Sum(time => BreakTimeHours));

            return missingHours;
        }

        public int GetMaxTimeCao(Employee employee, Department department, DateOnly startDate, DateOnly endDate, int startingHour)
        {
            List<int> hours = new List<int>();
            int age = DateTime.Now.Year - employee.DateOfBirth.Year;
            if (!(employee.DateOfBirth.Month <= DateTime.Now.Month && employee.DateOfBirth.Day <= DateTime.Now.Day))
            {
                age--;
            }

            List<Shift> workingShiftsThisWeek = employee.Shifts
                .Where(e => e.Start.Date <= startDate.AddDays(6).ToDateTime(new TimeOnly()))
                .Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()))
                .ToList();
            int workingHours = GetWorkingHours(workingShiftsThisWeek);
            if (age >= 18)
            {

                int maxShiftLenght = MaxShiftLengthAdult - GetWorkingHours(workingShiftsThisWeek.
                    Where(e => e.Start.Date == endDate.ToDateTime(new TimeOnly())));
                int maxWeekHoursLeft = MaxWeeklyHoursAdult - GetWorkingHours(workingShiftsThisWeek);
                hours.Add(maxWeekHoursLeft);
                hours.Add(maxShiftLenght);
            }

            else if (age < 16)
            {
                if (department == Department.Kassa) return 0;

                int maxWeekHours;
                int maxTimeWithSchoolHours;
                int maxHoursTillMaxTime;

                if (!((endDate.ToDateTime(new TimeOnly()) - startDate.ToDateTime(new TimeOnly())).Days < MaxWorkingDaysUnderSixteen))
                {
                    return 0;
                }
                if (employee.SchoolSchedules == null || !employee.SchoolSchedules.Any(e => e.Date >= startDate && e.Date <= startDate.AddDays(6)))
                {
                    maxWeekHours = MaxWeeklyHoursUnderSixteen - workingHours;
                    maxTimeWithSchoolHours = MaxHoursWithSchoolUnderSixteen;
                }
                else
                {
                    maxWeekHours = MaxWeeklyHoursSchoolweekUnderSixteen - workingHours;
                    maxTimeWithSchoolHours = MaxHoursWithSchoolUnderSixteen - GetSchoolHours(endDate, employee) - GetWorkingHours(workingShiftsThisWeek.
                    Where(e => e.Start.Date == endDate.ToDateTime(new TimeOnly())));
                }

                if (new TimeOnly(startingHour, 00, 00) >= _maxWorkTimeForUnderSixteen) { return 0; }

                maxHoursTillMaxTime = _maxWorkTimeForUnderSixteen.Hour - startingHour;

                hours.Add(maxWeekHours);
                hours.Add(maxTimeWithSchoolHours);
                hours.Add(maxHoursTillMaxTime);
            }

            else // age 16 and 17
            {
                int maxTimeWithSchoolHours;
                int maxAllowedHours = MaxWeeklyHoursAlmostAdult * TimeframeInWeeksMaxWeeklyHoursAlmostAdult;
                int amountOfDaysLookedAtForMaxAllowedHours = TimeframeInWeeksMaxWeeklyHoursAlmostAdult * 7;

                int workedHoursInTimeframe = GetWorkingHours(_context.Shifts
                .Where(e => e.Start.Date <= endDate.ToDateTime(new TimeOnly()))
                .Where(e => e.Start.Date >= endDate.AddDays(amountOfDaysLookedAtForMaxAllowedHours - 1).ToDateTime(new TimeOnly()))
                .ToList());

                var maxhoursWithAverage = maxAllowedHours + workedHoursInTimeframe;
                if (employee.SchoolSchedules.Any(e => e.Date >= startDate && e.Date <= startDate.AddDays(6)))
                {
                    maxTimeWithSchoolHours = MaxHoursWithSchoolAlmostAdult;
                }
                else
                {
                    maxTimeWithSchoolHours = MaxHoursWithSchoolAlmostAdult - GetSchoolHours(endDate, employee) - GetWorkingHours(workingShiftsThisWeek.
                    Where(e => e.Start.Date == endDate.ToDateTime(new TimeOnly())));
                }
                hours.Add(maxTimeWithSchoolHours);
                hours.Add(maxhoursWithAverage);
                hours.Add(maxAllowedHours);
            }
            hours.Sort();
            return hours.First();
        }

        private int GetSchoolHours(DateOnly scheduleDate, Employee employee)
        {
            SchoolSchedule? schedule = employee.SchoolSchedules.FirstOrDefault(e => e.Date == scheduleDate);
            if (schedule == null) { return 0; }

            return (int)Math.Round(schedule.DurationInHours);
        }
    }
}
