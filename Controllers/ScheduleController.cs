using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

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

        private void ScheduleShift(Department department, DateOnly scheduledate, Employee employee)
        {
            Availability availability = employee.Availabilities.Where(e => e.Date == scheduledate).FirstOrDefault();
        }

        private bool ScheduleConcurrentShift(Department department, DateOnly scheduledate, Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
