using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace BumboApp.Controllers
{
    public class ScheduleController : MainController
    {
        // will need to be in a json file   
        private int maxShiftLengthAdult = 12;
        private int maxWeeklyHoursAdult = 60;

        private int maxWeeklyHoursAlmostAdult = 40;
        private int timeframeInWeeksMaxWeeklyHoursAlmostAdult = 4;
        private int maxHoursWithSchoolAlmostAdult = 9;

        private int maxWeeklyHoursUnderSixteen = 40;
        private int maxWeeklyHoursSchoolweekUnderSixteen = 12;
        private int maxHoursWithSchoolUnderSixteen = 8;
        private int maxWorkingDaysUnderSixteen = 5;
        private TimeOnly maxWorkTimeForUnderSixteen = new TimeOnly(19,00,00);

        private float breakTimeHours = (float)0.5;
        private int[] breakTimes = { 4, 8 };

        public IActionResult Index(DateOnly date)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeSchedule(DateOnly startDate)
        {
            Console.WriteLine("wow1");
            Prognosis prognosis = Context.Prognoses.Where(e => e.Date == startDate).FirstOrDefault();
            if (prognosis == null)
            {
                return NotifyErrorAndRedirect("Geen prognose om het rooster te maken", "Index"); //route to prognose page?
            }
            List<Department> departmentList = new List<Department> { Department.Kassa, Department.Vers, Department.Vakkenvullen };
            DateOnly endDate = startDate.AddDays(6);
            Console.WriteLine("wow1");
            foreach (Department department in departmentList)
            {
                for (DateOnly scheduledate = startDate; scheduledate <= endDate; scheduledate.AddDays(1))
                {
                    ScheduleDepartmentDay(department, scheduledate, startDate);
                    if(!PrognoseHoursHit(department, scheduledate)) {InsertEmptyShifts(department, scheduledate); }
                }
            }
            Console.WriteLine("wow2");
            List<Employee> employees = (List<Employee>)Context.Employees
                .Where(e => e.ContractHours > GetWorkingHours( 
                e.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= endDate.ToDateTime(new TimeOnly()))));

            List<Employee> managers = Context.Employees.Where(e => e.User.UserName == Role.Manager.ToString()).ToList();
            int weekNumber = new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(startDate.ToDateTime(new TimeOnly()), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            Console.WriteLine("wow3");
            foreach (Employee e in employees)
            {
                string name = e.FirstName + " " + e.LastName + " ";
                foreach (Employee m in managers)
                {
                    Context.Notifications.Add(new Notification()
                    {
                        Title = name + "heeft te weinig uren in week " + weekNumber,
                        Description = name + "heeft minder dan " + e.ContractHours + " uren in week " + weekNumber,
                        Employee = m
                    });
                }
            }
            Console.WriteLine("wow4");
            try
            {
                Context.SaveChanges(); //more saves between edits or just in the end?
            }
            catch(Exception e) { return NotifyErrorAndRedirect("er is een probleem opgetreden", "Index"); }
            return RedirectToAction("Index",startDate);
        }

        private void InsertEmptyShifts(Department department, DateOnly scheduledate)
        {
            throw new NotImplementedException();
        }

        private void ScheduleDepartmentDay(Department department, DateOnly scheduledate, DateOnly startDate)
        {
            List<Employee> employees = Context.Employees.Include(e => e.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= scheduledate.ToDateTime(new TimeOnly())))
                .Include(e => e.SchoolSchedules.Where(e => e.Date == scheduledate))
                //.Include(e => e.leaveRequests.Where(e => e.StartDate <= scheduledate.ToDateTime(new TimeOnly()) && e.EndDate >= scheduledate.ToDateTime(new TimeOnly()))
                //.Where(e => e.Status == Status.Geaccepteerd))
                //.Include(e => e.Availabilities.Where(e => e.Date == scheduledate)).
                //OrderBy(e => e.ContractHours/GetWorkingHours(
                //e.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= scheduledate.ToDateTime(new TimeOnly())))).
                //ThenByDescending(e => e.ContractHours).
                .ToList();
            int index = 0;
            while (true) //look out that it is not infinite
            { 
                Employee employee = employees.ElementAt(index);
                if (department == Department.Kassa && OpeningInCashRegister(scheduledate))
                {
                    bool result = ScheduleConcurrentShift(department, scheduledate,startDate , employee);
                    index++;
                    if (result) index = 0;
                }
                else
                {
                    ScheduleShift(department, scheduledate, startDate, employee);
                    index++;
                }
                if (index >= employees.Count || PrognoseHoursHit(department,scheduledate)) break;
            }
            return;
        }

        //works if shifts start and end on a full hour
        private int GetWorkingHours(IEnumerable<Shift> shifts)
        {
            float hours = 0;
            foreach (Shift shift in shifts)
            {
                TimeSpan time = shift.End - shift.Start; 
                hours += time.Hours;
                foreach(int breakTime in breakTimes)
                {
                    if(time.Hours > breakTime)
                    {
                        hours -= breakTimeHours;
                    }
                }
            }
            return (int)Math.Ceiling(hours);
        }

        private bool PrognoseHoursHit(Department department, DateOnly scheduledate)
        {
            Prognosis prognosis= Context.Prognoses.Where(e => e.Date == scheduledate && e.Department == department).FirstOrDefault();
            List<Shift> departmentDayShifts = (List<Shift>)Context.Shifts
                .Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Department == department)
                .ToList();
            if (prognosis == null)
            {
                NotifyErrorAndRedirect("Geen prognose om het rooster te maken", "Index"); //route to prognose page?
            }
            if (prognosis.NeededHours >= GetWorkingHours(departmentDayShifts))
            {
                return true;
            }
            return false;
        }

        private bool OpeningInCashRegister(DateOnly scheduledate)
        {
            TimeOnly oTime = (TimeOnly)Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).FirstOrDefault().OpeningTime;
            TimeOnly cTime = (TimeOnly)Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).FirstOrDefault().ClosingTime;
            int openingHour = oTime.Hour;
            int closingHour = cTime.Hour;
            if (cTime.Minute > 0) { closingHour++; }
            List<Shift> shifts = Context.Shifts
                    .Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly())
                    && e.Department == Department.Kassa).OrderBy(e => e.Start.Hour).ToList();
            Shift previous = null;
            foreach (Shift shift in shifts)
            {
                if (previous == null)
                {
                    if (shift.Start.Hour == openingHour)
                    {
                        previous = shift;
                        continue;
                    }
                    return true;
                }
                if (previous.End.Hour >= shift.Start.Hour)
                {
                    previous = shift;
                    continue;
                }
                return true;
            }
            if (!shifts.Any())
            {
                return true;
            }
            if(shifts.Last().End.Hour == closingHour)
            {
                return false;
            }
            return true;
        }

        //schedules shift if possible
        private void ScheduleShift(Department department, DateOnly scheduledate, DateOnly startDate, Employee employee)
        {
            Availability availability = employee.Availabilities.Where(e => e.Date == scheduledate).FirstOrDefault();
            OpeningHour openingHour = Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).FirstOrDefault();
            int startinghour;

            //checks if employee is available and sets starting time
            if (availability == null)
            {
                startinghour = openingHour.OpeningTime.Value.Hour;
            }
            else if ((availability.EndTime - availability.StartTime).Hours == 0)
            {
                startinghour = availability.StartTime.Hour;
                if (availability.StartTime.Minute > 0) { startinghour++; }
            }
            else return;

            int maxTimeCAO = getMaxTimeCAO(employee, startDate, scheduledate, startinghour);
            int maxTimePrognose = getMaxTimePrognose(department, scheduledate);
            int maxTimeContract = getMaxTimeContract(employee, startDate, scheduledate);
            int maxTimeAvailable = (availability.EndTime - availability.StartTime).Hours;
            List<int> maxhours = new List<int> { maxTimeCAO, maxTimePrognose, maxTimeContract,maxTimeAvailable };
            maxhours.Sort();

            if (maxhours.First() > 0)
            {
                int endTime = maxhours.First() + startinghour;
                if (endTime > 24) { endTime = 24; } //closing maken of gwn houden
                if (department == Department.Kassa && endTime > openingHour.ClosingTime.Value.Hour + 1) { return; }
                Context.Add(
                    new Shift()
                    {
                        Department = department,
                        Employee = employee,
                        Start = scheduledate.ToDateTime(new TimeOnly(startinghour, 00, 00)),
                        End = scheduledate.ToDateTime(new TimeOnly(startinghour + maxhours.First(), 00, 00)),
                        IsFinal = false
                    });
            }
            return;
        }

        private bool ScheduleConcurrentShift(Department department, DateOnly scheduledate, DateOnly startDate, Employee employee)
        {
            Availability availability = null;
            try
            {
                availability = employee.Availabilities.Where(e => e.Date == scheduledate).First();
            }
            catch (Exception ex) { Console.WriteLine("empty"); }
            int availableFrom;
            int availableTill;
            TimeOnly cTime = (TimeOnly)Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).FirstOrDefault().ClosingTime;
            int startingHour = getNextEmptySpot(department, scheduledate);
            int closingHour = cTime.Hour;
            if (cTime.Minute > 0) { closingHour++; }



            if(availability != null)
            {
                availableFrom = availability.StartTime.Hour;
                availableTill = availability.EndTime.Hour;
                if (availability.StartTime.Minute > 0) { availableFrom++; }
                if (availability.EndTime.Minute > 0) { availableTill++; }

                if (!(startingHour < availableFrom && startingHour > availableTill))
                {
                    return false;
                }
            }
            else
            {
                availableFrom = 0;
                availableTill = 24;
            }

            int maxTimeCAO = getMaxTimeCAO(employee, startDate, scheduledate, startingHour);
            int maxTimePrognose = getMaxTimePrognose(department, scheduledate);
            int maxTimeContract = getMaxTimeContract(employee, startDate, scheduledate);
            int maxTimeAvailable = availableTill - availableFrom;
            int maxTimeTillClose = closingHour - startingHour;

            List<int> maxhours = new List<int> { maxTimeCAO, maxTimePrognose, maxTimeContract, maxTimeAvailable, maxTimeTillClose };
            maxhours.Sort();

            if (maxhours.First() > 0)
            {
                int endTime = maxhours.First() + startingHour;
                Context.Add(
                    new Shift()
                    {
                        Department = department,
                        Employee = employee,
                        Start = scheduledate.ToDateTime(new TimeOnly(startingHour, 00, 00)),
                        End = scheduledate.ToDateTime(new TimeOnly(startingHour + maxhours.First(), 00, 00)),
                        IsFinal = false
                    });
                return true;
            }
            return false;
        }

        private int getNextEmptySpot(Department department, DateOnly scheduledate)
        {
            TimeOnly oTime = (TimeOnly)Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).FirstOrDefault().OpeningTime;
            TimeOnly cTime = (TimeOnly)Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).FirstOrDefault().ClosingTime;
            int openingHour = oTime.Hour;
            int closingHour = cTime.Hour;
            if (cTime.Minute > 0) { closingHour++; }
            List<Shift> shifts = Context.Shifts
                    .Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly())
                    && e.Department == Department.Kassa).OrderBy(e => e.Start.Hour).ToList();
            Shift previous = null;
            if (!shifts.Any())
            {
                return openingHour;
            }
            foreach (Shift shift in shifts)
            {
                if (previous == null)
                {
                    if (shift.Start.Hour == openingHour)
                    {
                        previous = shift;
                        continue;
                    }
                    return openingHour;
                }
                if (previous.End.Hour >= shift.Start.Hour)
                {
                    previous = shift;
                    continue;
                }
            }
            if (previous.End.Hour == closingHour)
            {
                return closingHour;
            }
            return previous.End.Hour;
        }

        private int getMaxTimeContract(Employee employee, DateOnly startDate, DateOnly scheduleDate)
        {
            int hours = employee.ContractHours;
            List<Shift> shifts = (List<Shift>)employee.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= scheduleDate.ToDateTime(new TimeOnly()));
            if (shifts.Count == 0) { return hours; }
            else
            {
                return hours - GetWorkingHours(shifts);
            }
        }

        private int getMaxTimePrognose(Department department, DateOnly scheduledate)
        {
            Prognosis prognosis = null;
            try
            {
                prognosis = Context.Prognoses.Where(e => e.Date == scheduledate && e.Department == department).First();
            }
            catch (Exception ex) { }
            
                List<Shift> departmentDayShifts = (List<Shift>)Context.Shifts
                .Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Department == department)
                .ToList();
            if (prognosis == null)
            {
                NotifyErrorAndRedirect("Geen prognose om het rooster te maken", "Index"); //route to prognose page?
                RedirectToAction("Index", "Home");
                return 0;
            }
            return (int)Math.Ceiling(prognosis.NeededHours - GetWorkingHours(departmentDayShifts));
        }

        private int getMaxTimeCAO(Employee employee, DateOnly startDate, DateOnly scheduledate, int startinghour)
        {
            List<int> hours = new List<int>();
            int age = DateTime.Now.Year - employee.DateOfBirth.Year;
            if( !(employee.DateOfBirth.Month <= DateTime.Now.Month && employee.DateOfBirth.Day <= DateTime.Now.Day))
            {
                age--;
            }
            List<Shift> workingShiftsThisWeek = employee.Shifts
                .Where(e => e.Start.Date <= scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()))
                .ToList();
            int workingHours = GetWorkingHours(workingShiftsThisWeek);
            if ( age >= 18 )
            {

                int maxShiftLenght = maxShiftLengthAdult - GetWorkingHours(workingShiftsThisWeek.
                    Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly())));
                int maxWeekHoursLeft = maxWeeklyHoursAdult - GetWorkingHours(workingShiftsThisWeek);
                hours.Add(maxWeekHoursLeft);
                hours.Add(maxShiftLenght);
            }
            else if(age < 16)
            {

                int maxWeekHours;
                int maxTimeWithSchoolHours;
                int maxHoursTillMaxTime;
                if(!((scheduledate.ToDateTime(new TimeOnly()) - startDate.ToDateTime(new TimeOnly())).Days < maxWorkingDaysUnderSixteen))
                {
                    int workingDays = 0;
                    for(DateTime day = startDate.ToDateTime(new TimeOnly()); startDate <= scheduledate; day.AddDays(1))
                    {
                        if(workingShiftsThisWeek.Where(e => e.Start.Date == day.Date).Any())
                        {
                            workingDays++;
                        }
                    }
                    if(workingDays >= maxWorkingDaysUnderSixteen) { return 0; }
                }
                if (!employee.SchoolSchedules.Where(e => e.Date >= startDate && e.Date <= startDate.AddDays(6)).Any()) // magic number
                {
                    maxWeekHours = maxWeeklyHoursSchoolweekUnderSixteen - workingHours;
                    maxTimeWithSchoolHours = maxHoursWithSchoolUnderSixteen;
                }
                else
                {
                    maxWeekHours = maxWeeklyHoursSchoolweekUnderSixteen - workingHours;
                    maxTimeWithSchoolHours = maxHoursWithSchoolUnderSixteen - GetSchoolHours(scheduledate, employee) -GetWorkingHours(workingShiftsThisWeek.
                    Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly())));
                }

                if(new TimeOnly(startinghour,00,00) >= maxWorkTimeForUnderSixteen) { return 0; }
                else { maxHoursTillMaxTime = maxWorkTimeForUnderSixteen.Hour - startinghour; }

                hours.Add(maxWeekHours);
                hours.Add(maxTimeWithSchoolHours);
                hours.Add(maxHoursTillMaxTime);
            }
            else
            {
                int maxWeekHours;
                int maxTimeWithSchoolHours;
                int maxhoursWithAverage;
                int maxAllowedHours = maxWeeklyHoursAlmostAdult * timeframeInWeeksMaxWeeklyHoursAlmostAdult;
                int amountOfDaysLookedAtForMaxAllowedHours = timeframeInWeeksMaxWeeklyHoursAlmostAdult * 7;

                int workedHoursInTimeframe = GetWorkingHours(Context.Shifts
                .Where(e => e.Start.Date <= scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Start.Date >= scheduledate.AddDays(amountOfDaysLookedAtForMaxAllowedHours - 1).ToDateTime(new TimeOnly()))
                .ToList());

                maxhoursWithAverage = maxAllowedHours + workedHoursInTimeframe;            
                if (employee.SchoolSchedules.Where(e => e.Date >= startDate && e.Date <= startDate.AddDays(6)).Any()) // magic number
                {
                    maxTimeWithSchoolHours = maxHoursWithSchoolAlmostAdult;
                }
                else
                {
                    maxTimeWithSchoolHours = maxHoursWithSchoolAlmostAdult - GetSchoolHours(scheduledate, employee) - GetWorkingHours(workingShiftsThisWeek.
                    Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly())));
                }
            }
            hours.Sort();
            return hours.First();
        }

        private int GetSchoolHours(DateOnly scheduledate, Employee employee)
        {
            SchoolSchedule schedule = employee.SchoolSchedules.Where(e => e.Date == scheduledate).First();
            if (schedule == null) { return 0; }
            else
            {
                return (int)Math.Ceiling(schedule.DurationInHours);
            }
        }
    }
}
