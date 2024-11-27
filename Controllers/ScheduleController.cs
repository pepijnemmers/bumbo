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
        public IActionResult Index(DateOnly date)
        {
            return View();
        }

        public IActionResult Create(DateOnly startDate)
        {
            Prognosis prognosis = Context.Prognoses.Where(e => e.Date == startDate).FirstOrDefault();
            if (prognosis == null)
            {
                NotifyErrorAndRedirect("Geen prognose om het rooster te maken", "Index"); //route to prognose page?
            }
            List<Department> departmentList = new List<Department> { Department.Kassa, Department.Vers, Department.Vakkenvullen };
            DateOnly endDate = startDate.AddDays(6);
            foreach (Department department in departmentList)
            {
                for (DateOnly scheduledate = startDate; scheduledate <= endDate; scheduledate.AddDays(1))
                {
                    ScheduleDepartmentDay(department, scheduledate, startDate);
                    if(!PrognoseHoursHit(department, scheduledate)) {InsertEmptyShifts(department, scheduledate); }
                }
            }
            List<Employee> employees = (List<Employee>)Context.Employees
                .Where(e => e.ContractHours > GetWorkingHours( 
                e.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= endDate.ToDateTime(new TimeOnly()))));

            List<Employee> managers = Context.Employees.Where(e => e.User.Role == Role.Manager).ToList();
            int weekNumber = new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(startDate.ToDateTime(new TimeOnly()), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

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
            List<Employee> employees = Context.Employees.Include(e => e.Shifts
                .Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= scheduledate.ToDateTime(new TimeOnly())))
                .Include(e => e.DateOfBirth)
                .Include(e => e.SchoolSchedules.Where(e => e.Date == scheduledate))
                .Include(e => e.leaveRequests.Where(e => e.StartDate <= scheduledate.ToDateTime(new TimeOnly()) && e.EndDate >= scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Status == Status.Geaccepteerd))
                .Include(e => e.Availabilities.Where(e => e.Date == scheduledate)).
                OrderBy(e => e.ContractHours/GetWorkingHours(
                e.Shifts.Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) && e.End.Date >= scheduledate.ToDateTime(new TimeOnly())))).
                ThenByDescending(e => e.ContractHours).
                ToList();
            int index = 0;
            while (true) //look out that it is not infinite
            { 
                Employee employee = employees.ElementAt(index);
                if (department == Department.Kassa && OpeningInCashRegister(scheduledate))
                {
                    bool result = ScheduleConcurrentShift(department, scheduledate, employee);
                    index++;
                    if (result) index = 0;
                }
                else
                {
                    ScheduleShift(department, scheduledate, employee);
                    index++;
                }
                if (index >= employees.Count || PrognoseHoursHit(department,scheduledate)) break;
            }
            return;
        }

        //works if shifts start and end on a full hour
        private int GetWorkingHours(IEnumerable<Shift> shifts)
        {
            int hours = 0;
            foreach (Shift shift in shifts)
            {
                TimeSpan time = shift.End - shift.Start; 
                hours += time.Hours;
            }
            return hours;
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
            if(shifts.Last().End.Hour == closingHour)
            {
                return false;
            }
            return true;
        }

        //schedules shift if possible
        private void ScheduleShift(Department department, DateOnly scheduledate, Employee employee)
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

            int maxTimeCAO = getMaxTimeCAO(employee,scheduledate, startinghour);
            int maxTimePrognose = getMaxTimePrognose(department, scheduledate);
            int maxTimeContract = getMaxTimeContract(employee);
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

        private bool ScheduleConcurrentShift(Department department, DateOnly scheduledate, Employee employee)
        {
            Availability availability = employee.Availabilities.Where(e => e.Date == scheduledate).FirstOrDefault();
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

            int maxTimeCAO = getMaxTimeCAO(employee, scheduledate, startingHour);
            int maxTimePrognose = getMaxTimePrognose(department, scheduledate);
            int maxTimeContract = getMaxTimeContract(employee);
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
            }


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

        private int getMaxTimeContract(Employee employee)
        {
            throw new NotImplementedException();
        }

        private int getMaxTimePrognose(Department department, DateOnly scheduledate)
        {
            throw new NotImplementedException();
        }

        private int getMaxTimeCAO(Employee employee, DateOnly scheduledate, int startinghour)
        {
            throw new NotImplementedException();
        }
    }
}
