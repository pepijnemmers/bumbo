using BumboApp.Models;
using System.Globalization;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BumboApp.Helpers;

namespace BumboApp.Controllers
{
    public class ScheduleController : MainController
    {
        private readonly UserManager<User> _userManager;
        private readonly MaxScheduleTimeCalculationHelper _maxScheduleTimeCalculationHelper;
        public ScheduleController(UserManager<User> userManager)
        {
            _userManager = userManager;
            _maxScheduleTimeCalculationHelper = new(Context);
        }

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
            Prognosis? prognosis = Context.Prognoses.FirstOrDefault(e => e.Date == startDate);
            if (prognosis == null)
            {
                return NotifyErrorAndRedirect("Geen prognose om het rooster te maken.", "Index","Prognoses");
            }

            List<Department> departmentList = new List<Department> { Department.Kassa, Department.Vers, Department.Vakkenvullen };
            DateOnly endDate = startDate.AddDays(6);

            /// when testing comment out the code between the comments (because there are already 2 shifts in the Db)
            if (Context.Shifts
                .Where(e => e.Start.Date >= startDate.ToDateTime(new TimeOnly()) &&
                e.End.Date <= endDate.ToDateTime(new TimeOnly())).Any())
            {
                return NotifyErrorAndRedirect("Er is al een rooster voor deze week.", "Index");
            }
            ///so till this point

            foreach (Department department in departmentList)
            {
                for (DateOnly scheduledate = startDate; scheduledate <= endDate; scheduledate = scheduledate.AddDays(1))
                {
                    ScheduleDepartmentDay(department, scheduledate, startDate); 
                    if (!PrognoseHoursHit(department, scheduledate)) { InsertEmptyShifts(department, scheduledate); }
                }
            }
            List<Employee> employees = Context.Employees
                .Include(e => e.Shifts)
                .Include(e => e.User)
                .ToList();
            List<Employee> managers = employees.Where(e => GetUserRoleAsync(e.User.Id).Result == Role.Manager).ToList();
            int weekNumber = new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(startDate.ToDateTime(new TimeOnly()), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            foreach (Employee e in employees)
            {
                if (managers.Contains(e)) continue;
                if (e.ContractHours >
                    _maxScheduleTimeCalculationHelper.GetWorkingHours(e.Shifts.Where(em => em.Start.Date >= startDate.ToDateTime(new TimeOnly())
                    && em.End.Date <= endDate.ToDateTime(new TimeOnly())).ToList()) && 
                    GetUserRoleAsync(e.User.Id).Result != Role.Manager)
                {
                    string name = e.FirstName + " " + e.LastName + " ";
                    foreach (Employee m in managers)
                    {
                        Context.Notifications.Add(new Notification()
                        {
                            SentAt = DateTime.Now,
                            Title = name + "heeft te weinig uren in week " + weekNumber + ".",
                            Description = name + "heeft minder dan " + e.ContractHours + " uren in week " + weekNumber + ".",
                            Employee = m
                        });
                    }
                }
            }
            try
            {
                Context.SaveChanges();
            }
            catch (Exception e) { return NotifyErrorAndRedirect("Er is een probleem opgetreden.", "Index"); }
            return RedirectToAction("Index",new {startDate = startDate.ToString()});
        }

        private void InsertEmptyShifts(Department department, DateOnly scheduledate)
        {
            OpeningHour? openingHour = Context.OpeningHours.FirstOrDefault(e => e.WeekDay == scheduledate.DayOfWeek);
            if (openingHour == null)
            {
                return;
            }
            TimeOnly oTime = openingHour.OpeningTime.GetValueOrDefault();
            TimeOnly cTime = openingHour.ClosingTime.GetValueOrDefault();
            int startingHour = oTime.Hour;
            int closingHour = cTime.Hour;
            if (cTime.Minute > 0) { closingHour++; }

            while (!PrognoseHoursHit(department, scheduledate))
            {
                int missedTime = _maxScheduleTimeCalculationHelper.GetMaxTimePrognose(department, scheduledate);
                int maxTime = startingHour + missedTime;

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
                try
                {
                    Context.SaveChanges();
                } catch(Exception ex) { break; }
            }
            return;
        }

        private void ScheduleDepartmentDay(Department department, DateOnly scheduledate, DateOnly startDate)
        {
            List<Employee> employees = Context.Employees.Include(e => e.Shifts)
                .Include(e => e.User)
                .Include(e => e.SchoolSchedules)
                .Include(e => e.leaveRequests)
                .Include(e => e.Availabilities)
                .Where(e => e.EndOfEmployment == null)
                .ToList();

            employees = employees.OrderBy(e => e.ContractHours / _maxScheduleTimeCalculationHelper.GetWorkingHoursNoZero(e.Shifts.Where(s => s.Start.Date <= startDate.ToDateTime(new TimeOnly()) && s.End.Date >= scheduledate.ToDateTime(new TimeOnly()))))
            .ThenByDescending(e => e.ContractHours).ToList();

            int index = 0;
            //true if not cash register or it is cash register but the whole list is looped and prognose hours not hit yet
            bool cantFindConcurrentForRegister = department != Department.Kassa; 
            while (true)
            {
                if (PrognoseHoursHit(department, scheduledate)) break;
                if (index >= employees.Count)
                {
                    if (cantFindConcurrentForRegister) break;
                    
                    cantFindConcurrentForRegister = true;
                    index = 0;
                }
                Employee employee = employees.ElementAt(index);
                if (GetUserRoleAsync(employee.User.Id).Result == Role.Manager)
                {
                    index++;
                    continue;
                }
                if (employee.leaveRequests.Any(e => e.Status == Status.Geaccepteerd && e.StartDate < scheduledate.ToDateTime(new TimeOnly()) && e.EndDate > scheduledate.ToDateTime(new TimeOnly())))
                {
                    index++;
                    continue;
                }
                if (employee.Shifts.Any(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly()) && e.End.Date == scheduledate.ToDateTime(new TimeOnly())))
                {
                    index++;
                    continue;
                }
                if (!cantFindConcurrentForRegister && department == Department.Kassa && OpeningInCashRegister(scheduledate))
                {
                    index = ScheduleConcurrentShift(department, scheduledate,startDate , employee) ? 0 : index + 1;
                    continue;
                }
                    ScheduleShift(department, scheduledate, startDate, employee);
                    index++;
            }
            return;
        }

        private bool PrognoseHoursHit(Department department, DateOnly scheduledate)
        {
            Prognosis? prognosis= Context.Prognoses.FirstOrDefault(e => e.Date == scheduledate && e.Department == department);
            List<Shift> departmentDayShifts = Context.Shifts
                .Where(e => e.Start.Date == scheduledate.ToDateTime(new TimeOnly()))
                .Where(e => e.Department == department)
                .ToList();
            if (prognosis == null)
            {
                return true;
            }
            if ((int)Math.Round(prognosis.NeededHours) >= _maxScheduleTimeCalculationHelper.GetWorkingHours(departmentDayShifts))
            {
                return false;
            }
            return true;
        }

        private bool OpeningInCashRegister(DateOnly scheduleDate)
        {
            OpeningHour? times = Context.OpeningHours.FirstOrDefault(e => e.WeekDay == scheduleDate.DayOfWeek);
            if (times == null) { return false; }

            TimeOnly oTime = times.OpeningTime.GetValueOrDefault();
            TimeOnly cTime = times.ClosingTime.GetValueOrDefault();

            int openingHour = oTime.Hour;
            int closingHour = cTime.Hour;
            if (cTime.Minute > 0) { closingHour++; }
            List<Shift> shifts = Context.Shifts
                    .Where(e => e.Start.Date == scheduleDate.ToDateTime(new TimeOnly())
                    && e.Department == Department.Kassa).OrderBy(e => e.Start.Hour).ToList();
            Shift? previous = null;
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

        private void ScheduleShift(Department department, DateOnly scheduleDate, DateOnly startDate, Employee employee)
        {
            Availability? availability = employee.Availabilities.FirstOrDefault(e => e.Date == scheduleDate);
            OpeningHour? openingHour = Context.OpeningHours.FirstOrDefault(e => e.WeekDay == scheduleDate.DayOfWeek);
            int startingHour;
            int maxTimeAvailable;

            LeaveRequest? leaveRequest = Context.LeaveRequests.Where(lr => lr.Employee == employee && lr.Status == Status.Geaccepteerd)
               .FirstOrDefault(lr => lr.StartDate.Date == scheduleDate.ToDateTime(new TimeOnly()) && lr.EndDate >= scheduleDate.ToDateTime(new TimeOnly()));

            if (openingHour == null) { return; }

            TimeOnly oTime = openingHour.OpeningTime.GetValueOrDefault();
            TimeOnly cTime = openingHour.ClosingTime.GetValueOrDefault();
            if (oTime.Hour == cTime.Hour) return;
            if (availability == null)
            {
                Models.StandardAvailability? standardAvailability = Context.StandardAvailabilities.FirstOrDefault(e => e.Employee == employee);
                if (standardAvailability == null)
                {
                    startingHour = oTime.Hour;
                    maxTimeAvailable = cTime.Minute > 0 ? cTime.Hour + 1 : cTime.Hour ;
                }
                else
                {
                    startingHour = standardAvailability.StartTime.Minute > 0 ? standardAvailability.StartTime.Hour + 1 : standardAvailability.StartTime.Hour;
                    maxTimeAvailable = standardAvailability.EndTime.Hour - startingHour;
                }
            }
            else 
            {
                maxTimeAvailable = (availability.EndTime - availability.StartTime).Hours;
                if (maxTimeAvailable <= 0)
                {
                    startingHour = availability.StartTime.Hour;
                    if (availability.StartTime.Minute > 0) { startingHour++; }
                }
                else { return; }
            }

            if (leaveRequest != null)
            {
                if (startingHour > leaveRequest.StartDate.Hour)
                {
                    if (startingHour < (leaveRequest.EndDate.Minute > 0 ? leaveRequest.EndDate.Hour + 1 : leaveRequest.EndDate.Hour)) { return; }
                }
            }

            int maxTimeCAO = _maxScheduleTimeCalculationHelper.GetMaxTimeCao(employee, department, startDate, scheduleDate, startingHour);
            int maxTimePrognose = _maxScheduleTimeCalculationHelper.GetMaxTimePrognose(department, scheduleDate);
            int maxTimeContract = _maxScheduleTimeCalculationHelper.GetMaxTimeContract(employee, startDate, scheduleDate);
            List<int> maxhours = new List<int> { maxTimeCAO, maxTimePrognose, maxTimeContract,maxTimeAvailable };
            maxhours.Sort();

            if (maxhours.First() > 0)
            {
                int endTime = maxhours.First() + startingHour;
                if(leaveRequest != null && leaveRequest.StartDate.Hour > startingHour)
                {
                    if(leaveRequest.StartDate.Hour < endTime) { endTime = leaveRequest.StartDate.Hour; }
                }
                if (startingHour >= endTime) { return; }
                if (endTime > 23) { endTime = 23; } //till closing or keep it like this
                if (department == Department.Kassa && endTime > cTime.Hour + 1) { return; }
                Context.Add(
                    new Shift()
                    {
                        Department = department,
                        Employee = employee,
                        Start = scheduleDate.ToDateTime(new TimeOnly(startingHour, 00, 00)),
                        End = scheduleDate.ToDateTime(new TimeOnly(endTime, 00, 00)),
                        IsFinal = false
                    });
                try
                {
                    Context.SaveChanges();
                }
                catch (Exception ex) { };
            }
            return;
        }

        private bool ScheduleConcurrentShift(Department department, DateOnly scheduleDate, DateOnly startDate, Employee employee)
        {
            Availability? availability = employee.Availabilities.FirstOrDefault(e => e.Date == scheduleDate);
            int availableFrom;
            int availableTill;
            OpeningHour? openingHour = Context.OpeningHours.FirstOrDefault(e => e.WeekDay == scheduleDate.DayOfWeek);

            LeaveRequest? leaveRequest = Context.LeaveRequests.Where(lr => lr.Employee == employee && lr.Status == Status.Geaccepteerd)
                .FirstOrDefault(lr => lr.StartDate.Date == scheduleDate.ToDateTime(new TimeOnly()) && lr.EndDate >= scheduleDate.ToDateTime(new TimeOnly()));

            if (openingHour == null) { return false; }
            TimeOnly cTime = openingHour.ClosingTime.GetValueOrDefault();
            int startingHour = GetNextEmptySpot(department, scheduleDate);
            int closingHour = cTime.Hour;
            if (cTime.Minute > 0) { closingHour++; }
            if (leaveRequest != null) 
            {
                if (startingHour > leaveRequest.StartDate.Hour) { return false; } ;
            }

            if(availability != null)
            {
                if ((availability.EndTime - availability.StartTime).Hours <= 0) { return false; }
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
                StandardAvailability? standardAvailability = Context.StandardAvailabilities.FirstOrDefault(e => e.Employee == employee && e.Day == scheduleDate.DayOfWeek);
                if(standardAvailability != null)
                {
                    availableFrom = (standardAvailability.StartTime.Minute > 0 ? standardAvailability.StartTime.Hour + 1 : standardAvailability.StartTime.Hour);
                    availableTill = standardAvailability.EndTime.Hour;
                }
                else
                {
                    availableFrom = openingHour.OpeningTime.Value.Hour; 
                    availableTill = closingHour;
                }
            }

            int maxTimeCao = _maxScheduleTimeCalculationHelper.GetMaxTimeCao(employee, department, startDate, scheduleDate, startingHour);
            int maxTimePrognose = _maxScheduleTimeCalculationHelper.GetMaxTimePrognose(department, scheduleDate);
            int maxTimeContract = _maxScheduleTimeCalculationHelper.GetMaxTimeContract(employee, startDate, scheduleDate);
            int maxTimeAvailable = availableTill - availableFrom;
            int maxTimeTillClose = closingHour - startingHour;

            List<int> maxhours = new List<int> { maxTimeCao, maxTimePrognose, maxTimeContract, maxTimeAvailable, maxTimeTillClose };
            maxhours.Sort();

            if (maxhours.First() > 0)
            {
                int endTime = maxhours.First() + startingHour;

                if (leaveRequest != null && leaveRequest.StartDate.Hour > startingHour)
                {
                    if (leaveRequest.StartDate.Hour < endTime) { endTime = leaveRequest.StartDate.Hour; }
                }

                if (endTime <= startingHour) { return false; }
                Context.Add(
                    new Shift()
                    {
                        Department = department,
                        Employee = employee,
                        Start = scheduleDate.ToDateTime(new TimeOnly(startingHour, 00, 00)),
                        End = scheduleDate.ToDateTime(new TimeOnly(endTime, 00, 00)),
                        IsFinal = false
                    });
                try
                {
                    Context.SaveChanges();
                }
                catch (Exception ex) { return false; }
                return true;
            }
            return false;
        }

        private int GetNextEmptySpot(Department department, DateOnly scheduleDate)
        {
            OpeningHour? openingTimes = Context.OpeningHours.FirstOrDefault(e => e.WeekDay == scheduleDate.DayOfWeek);
            if (openingTimes == null) { return 8; }

            TimeOnly oTime = openingTimes.OpeningTime.GetValueOrDefault();
            TimeOnly cTime = openingTimes.ClosingTime.GetValueOrDefault();

            int openingHour = oTime.Hour;
            int closingHour = cTime.Hour;

            if (cTime.Minute > 0) { closingHour++; }

            List<Shift> shifts = Context.Shifts
                    .Where(e => e.Start.Date == scheduleDate.ToDateTime(new TimeOnly())  && e.Department == Department.Kassa)
                    .OrderBy(e => e.Start.Hour)
                    .ToList();
            Shift? previous = null;

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

            if(previous == null) { return openingHour; }

            if (previous.End.Hour == closingHour)
            {
                return closingHour;
            }

            return previous.End.Hour;
        }
        
        public IActionResult DeleteWeek(int weekNumber, int year)
        {
            var deleteDate = new DateOnly(year, 1, 1).AddDays((weekNumber - 1) * 7);
            var shifts = Context.Shifts
                .Where(shift => DateOnly.FromDateTime(shift.Start) >= deleteDate
                                && DateOnly.FromDateTime(shift.Start) < deleteDate.AddDays(7))
                .ToList();
            
            if (shifts.Count == 0)
            {
                NotifyService.Error("Er zijn geen diensten gevonden voor de week");
                return RedirectToAction("Index", new { startDate = deleteDate.ToString("dd/MM/yyyy") });
            }

            try
            {
                Context.ShiftTakeOvers.RemoveRange(Context.ShiftTakeOvers.Where(takeOver => shifts.Contains(takeOver.Shift)));
                Context.Shifts.RemoveRange(shifts);
                Context.SaveChanges();
                NotifyService.Success("De shifts van de week zijn verwijderd");
                return RedirectToAction("Index", new { startDate = deleteDate.ToString("dd/MM/yyyy") });
            }
            catch
            {
                NotifyService.Error("Er is iets fout gegaan bij het verwijderen van de shifts");
                return RedirectToAction("Index", new { startDate = deleteDate.ToString("dd/MM/yyyy") });
            }
        }

        public IActionResult PublishWeek(int weekNumber, int year)
        {
            var publishDate = new DateOnly(year, 1, 1).AddDays((weekNumber - 1) * 7);
            var shifts = Context.Shifts
                .Where(shift => DateOnly.FromDateTime(shift.Start) >= publishDate
                                && DateOnly.FromDateTime(shift.Start) < publishDate.AddDays(7)
                                && !shift.IsFinal).Include(shift => shift.Employee)
                .ToList();

            if (shifts.Count == 0)
            {
                NotifyService.Error("Er zijn geen concept diensten gevonden voor de week");
                return RedirectToAction("Index", new { startDate = publishDate.ToString("dd/MM/yyyy") });
            }

            try
            {
                foreach (var shift in shifts)
                {
                    shift.IsFinal = true;
                    
                    // send notification to employee
                    if (shift.Employee != null)
                    {
                        Context.Notifications.Add(new Notification
                        {
                            Employee = shift.Employee,
                            Title = $"Dienst {shift.Department} toegevoegd - {shift.Start.ToString("dd-MM-yyyy")}",
                            Description = $"Er is een dienst voor jou toegevoegd op {shift.Start.ToString("dd-MM-yyyy")}",
                            SentAt = DateTime.Now,
                            HasBeenRead = false,
                            ActionUrl = $"/Schedule?startDate={shift.Start.ToString("dd/MM/yyyy")}"
                        });
                    }
                }
                
                Context.SaveChanges();
                NotifyService.Success("De concept diensten zijn gepubliceerd");
                return RedirectToAction("Index", new { startDate = publishDate.ToString("dd/MM/yyyy") });
            }
            catch
            {
                NotifyService.Error("Er is iets fout gegaan bij het publiceren van de diensten");
                return RedirectToAction("Index", new { startDate = publishDate.ToString("dd/MM/yyyy") });
            }
            
        }

    }
}
