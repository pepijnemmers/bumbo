﻿using BumboApp.Models;
using System.Globalization;
using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace BumboApp.Controllers
{

    public class ScheduleController : MainController
    {
        private UserManager<User> _userManager;
        public ScheduleController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        // will need to be in a json file eventually 
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

        public async Task<Role> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return (Role)Enum.Parse(typeof(Role), roles[0]);
            }
            return Role.Unknown;
        }

        public IActionResult Index(string? employeeNumber, string? startDate, bool dayView = true)

        {
            // Selected employee filter or logged in employee
            Employee? selectedEmployee = null;
            if (LoggedInUserRole == Role.Employee)
            {
                selectedEmployee = Context.Employees.FirstOrDefault(employee => employee.User.Id == LoggedInUserId);
            }
            else
            {
                selectedEmployee = employeeNumber != null && int.TryParse(employeeNumber, out _) ? Context.Employees.Find(int.Parse(employeeNumber)) : null;
            }
            
            // Day or week view and selected start date
            bool isDayView = selectedEmployee == null || dayView;
            var selectedStartDate = startDate != null 
                ? (isDayView ? DateOnly.FromDateTime(DateTime.Parse(startDate)) : GetMondayOfWeek(DateOnly.FromDateTime(DateTime.Parse(startDate))))
                : (isDayView ? DateOnly.FromDateTime(DateTime.Today) : GetMondayOfWeek(DateOnly.FromDateTime(DateTime.Today)));
            int weekNumber = ISOWeek.GetWeekOfYear(selectedStartDate.ToDateTime(new TimeOnly(12, 00)));
            
            // Get all employees for filter
            var employees = Context.Employees.ToList();
            
            // Get all shifts for selected date (based on day/week view) and selected/loggedin employee
            var shifts = Context.Shifts
                .Where(shift => isDayView
                    ? DateOnly.FromDateTime(shift.Start) == selectedStartDate 
                      && (selectedEmployee == null || shift.Employee == selectedEmployee) 
                      && (LoggedInUserRole != Role.Employee || shift.IsFinal)
                    : DateOnly.FromDateTime(shift.Start) >= selectedStartDate 
                      && DateOnly.FromDateTime(shift.Start) < selectedStartDate.AddDays(7) 
                      && (selectedEmployee == null || shift.Employee == selectedEmployee) 
                      && (LoggedInUserRole != Role.Employee || shift.IsFinal))
                .OrderBy(shift => shift.Department)
                .ToList();
            
            // Check if view is concept or final
            var viewIsConcept = shifts.Any(shift => !shift.IsFinal);
            
            // Create view model and return view
            var viewModel = new ScheduleViewModel()
            {
                Role = LoggedInUserRole,
                ViewIsConcept = viewIsConcept,
                SelectedStartDate = selectedStartDate,
                WeekNumber = weekNumber,
                Employees = employees,
                Shifts = shifts,
                SelectedEmployee = selectedEmployee,
                IsDayView = isDayView
            };
            return View(viewModel);
        }
        
        private DateOnly GetMondayOfWeek(DateOnly date)
        {
            var dayOfWeek = (int) date.DayOfWeek;
            var daysToMonday = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
            return date.AddDays(-daysToMonday);
        }

        public IActionResult Create()
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        [HttpPost]
        public IActionResult MakeSchedule(DateOnly startDate)
        {
            Prognosis prognosis = Context.Prognoses.Where(e => e.Date == startDate).FirstOrDefault();
            if (prognosis == null)
            {
                return NotifyErrorAndRedirect("Geen prognose om het rooster te maken", "Index"); //route to prognose page?
            }

            List<Department> departmentList = new List<Department> { Department.Kassa, Department.Vers, Department.Vakkenvullen };
            DateOnly endDate = startDate.AddDays(6);

            //for testing comment out the code between the comments (because there are already 2 shifts in the Db)
            if (Context.Shifts
                .Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) &&
                e.End.Date <= endDate.ToDateTime(new TimeOnly())).Any())
            {
                return NotifyErrorAndRedirect("er is al een rooster voor deze week", "Index");
            }
            //so till this point

            foreach (Department department in departmentList)
            {
                for (DateOnly scheduledate = startDate; scheduledate <= endDate; scheduledate = scheduledate.AddDays(1))
                {
                    ScheduleDepartmentDay(department, scheduledate, startDate); 
                    if (!PrognoseHoursHit(department, scheduledate)) { InsertEmptyShifts(department, scheduledate); }
                }
            }
            List<Employee> employees = Context.Employees
                .Include(e => e.Shifts).ToList();
            List<Employee> managers = employees.Where(e => GetUserRoleAsync(e.User.Id).Result == Role.Manager).ToList();
            int weekNumber = new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(startDate.ToDateTime(new TimeOnly()), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            foreach (Employee e in employees)
            {
                if (managers.Contains(e)) continue;
                if (e.ContractHours >
                    GetWorkingHours(e.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly())
                    && e.End.Date <= endDate.ToDateTime(new TimeOnly())).ToList()) && 
                    GetUserRoleAsync(e.User.Id).Result == Role.Manager)
                {
                    string name = e.FirstName + " " + e.LastName + " ";
                    foreach (Employee m in managers)
                    {
                        Context.Notifications.Add(new Notification()
                        {
                            SentAt = DateTime.Now,
                            Title = name + "heeft te weinig uren in week " + weekNumber,
                            Description = name + "heeft minder dan " + e.ContractHours + " uren in week " + weekNumber,
                            Employee = m
                        });
                    }
                }
            }
            try
            {
                Context.SaveChanges();
            }
            catch(Exception e) { return NotifyErrorAndRedirect("er is een probleem opgetreden", "Index"); }
            return RedirectToAction("Index",null,startDate.ToString());
        }

        private void InsertEmptyShifts(Department department, DateOnly scheduledate)
        {
            OpeningHour openingHour = Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).First();
            try
            {
                TimeOnly oTime = openingHour.OpeningTime.GetValueOrDefault();
                TimeOnly cTime = openingHour.ClosingTime.GetValueOrDefault();
                int startingHour = oTime.Hour;
                int closingHour = cTime.Hour;
                if (cTime.Minute > 0) { closingHour++; }
                while (!PrognoseHoursHit(department, scheduledate))
                {
                    int MissedTime = getMaxTimePrognose(department, scheduledate);
                    int maxTime = startingHour + (int)MissedTime;

                    if (maxTime > closingHour) { maxTime = closingHour; }
                    if (startingHour >= maxTime) { return; }
                    Context.Add(
                        new Shift()
                        {
                            Department = department,
                            Start = scheduledate.ToDateTime(new TimeOnly(startingHour, 00, 00)),
                            End = scheduledate.ToDateTime(new TimeOnly(maxTime, 00, 00)),
                            IsFinal = false,
                        });
                    Context.SaveChanges();
                }
            }
            catch (Exception ex) { return; }
            return;
        }

        private void ScheduleDepartmentDay(Department department, DateOnly scheduledate, DateOnly startDate)
        {
            List<Employee> employees = Context.Employees.Include(e => e.Shifts)
                .Include(e => e.User)
                .Include(e => e.SchoolSchedules)
                .Include(e => e.leaveRequests)
                .Include(e => e.Availabilities)
                .ToList();

            employees = employees.OrderBy(e => e.ContractHours / GetWorkingHoursNoZero(e.Shifts.Where(e => e.Start.Date <= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= scheduledate.ToDateTime(new TimeOnly()))))
            .ThenByDescending(e => e.ContractHours).ToList();

            int index = 0;
            //true if not cash register or it is cash register but the whole list is looped and prognose hours not hit yet
            bool cantFindConcurrentForRegister; 
            if(department == Department.Kassa)
            {
                cantFindConcurrentForRegister = false;
            }
            else { cantFindConcurrentForRegister= true; }
            while (true)
            {
                if (PrognoseHoursHit(department, scheduledate)) break;
                if (index >= employees.Count)
                {
                    if (cantFindConcurrentForRegister) break;
                    else
                    {
                        cantFindConcurrentForRegister = true;
                        index = 0;
                    }
                }
                Employee employee = employees.ElementAt(index);
                if (GetUserRoleAsync(employee.User.Id).Result == Role.Manager)
                {
                    index++;
                    continue;
                }
                if (employee.leaveRequests.Where(e => e.Status == Status.Geaccepteerd && e.StartDate <= scheduledate.ToDateTime(new TimeOnly()) && e.EndDate >= scheduledate.ToDateTime(new TimeOnly())).Any())
                {
                    index++;
                    continue;
                }
                if (employee.Shifts.Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly()) && e.End.Date == scheduledate.ToDateTime(new TimeOnly())).Any())
                {
                    index++;
                    continue;
                }
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
        private float GetWorkingHoursNoZero(IEnumerable<Shift> shifts)
        {
            float hours = 0;
            foreach (Shift shift in shifts)
            {
                TimeSpan time = shift.End - shift.Start;
                hours += time.Hours;
                foreach (int breakTime in breakTimes)
                {
                    if (time.Hours > breakTime)
                    {
                        hours -= breakTimeHours;
                    }
                }
            }
            if(hours == 0)
            {
                return 0.1F;
            }
            return hours;
        }

        private bool PrognoseHoursHit(Department department, DateOnly scheduledate)
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return true;
            }
            Prognosis prognosis= Context.Prognoses.Where(e => e.Date == scheduledate && e.Department == department).FirstOrDefault();
            List<Shift> departmentDayShifts = Context.Shifts
                .Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Department == department)
                .ToList();
            if (prognosis == null)
            {
                return true;
            }
            if ((int)Math.Round(prognosis.NeededHours) >= GetWorkingHours(departmentDayShifts))
            {
                return false;
            }
            return true;
        }

        private bool OpeningInCashRegister(DateOnly scheduledate)
        {
            OpeningHour times = Context.OpeningHours.Where(e => e.WeekDay == scheduledate.DayOfWeek).First();

            TimeOnly oTime;
            TimeOnly cTime;
            try
            {
                oTime = (TimeOnly)times.OpeningTime;
                cTime = (TimeOnly)times.ClosingTime;
            } catch (Exception ex) { return false; }

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
            int maxTimeAvailable;

            //checks if employee is available and sets starting time
            try
            {
                TimeOnly oTime = (TimeOnly)openingHour.OpeningTime;
                TimeOnly cTime = (TimeOnly)openingHour.ClosingTime;
                if (oTime.Hour == cTime.Hour) return;
            }
            catch (Exception ex) { return; }
            if (availability == null)
            {
                try
                {
                    startinghour = openingHour.OpeningTime.Value.Hour;
                    maxTimeAvailable = maxShiftLengthAdult;
                } catch (Exception ex) { return; }
            }
            else {
                maxTimeAvailable = (availability.EndTime - availability.StartTime).Hours;
                if (maxTimeAvailable == 0)
                {
                startinghour = availability.StartTime.Hour;
                if (availability.StartTime.Minute > 0) { startinghour++; }
                }
                else return;
                }

            int maxTimeCAO = getMaxTimeCAO(employee, department, startDate, scheduledate, startinghour);
            int maxTimePrognose = getMaxTimePrognose(department, scheduledate);
            int maxTimeContract = getMaxTimeContract(employee, startDate, scheduledate);
            List<int> maxhours = new List<int> { maxTimeCAO, maxTimePrognose, maxTimeContract,maxTimeAvailable };
            maxhours.Sort();

            if (maxhours.First() > 0)
            {
                int endTime = maxhours.First() + startinghour;
                if (endTime > 24) { endTime = 24; } //till closing or keep it like this
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
            catch (Exception ex) { }
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
                availableTill = 23;
            }

            int maxTimeCAO = getMaxTimeCAO(employee, department, startDate, scheduledate, startingHour);
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
            List<Shift> shifts = (List<Shift>)employee.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= scheduleDate.ToDateTime(new TimeOnly())).ToList();
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
                return 0;
            }
            float MissingHours = prognosis.NeededHours - GetWorkingHours(departmentDayShifts);
            foreach (int time in breakTimes)
            {
                MissingHours += breakTimeHours;
            }

            return (int)Math.Ceiling(MissingHours);
        }

        private int getMaxTimeCAO(Employee employee,Department department, DateOnly startDate, DateOnly scheduledate, int startinghour)
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
                    if(workingDays >= maxWorkingDaysUnderSixteen || department == Department.Kassa) { return 0; }
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
                int maxTimeWithSchoolHours;
                int maxhoursWithAverage;
                int maxAllowedHours = maxWeeklyHoursAlmostAdult * timeframeInWeeksMaxWeeklyHoursAlmostAdult;
                int amountOfDaysLookedAtForMaxAllowedHours = timeframeInWeeksMaxWeeklyHoursAlmostAdult * 7;

                int workedHoursInTimeframe = GetWorkingHours(Context.Shifts
                .Where(e => e.Start.Date <= scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Start.Date >= scheduledate.AddDays(amountOfDaysLookedAtForMaxAllowedHours - 1).ToDateTime(new TimeOnly()))
                .ToList());

                maxhoursWithAverage = maxAllowedHours + workedHoursInTimeframe;            
                if (employee.SchoolSchedules.Where(e => e.Date >= startDate && e.Date <= startDate.AddDays(6)).Any())
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
        public IActionResult DeleteWeek(DateOnly startDay)
        {
            return RedirectToAction("Index");

        }
    }
}
