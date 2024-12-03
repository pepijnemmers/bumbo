using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        private Employee _loggedInEmployee;
        private readonly int hoursPerDay = 8;
        public IActionResult Index(int? page, SortOrder? orderBy = SortOrder.Ascending, Status? selectedStatus = null)
        {
            _loggedInEmployee = GetLoggedInEmployee();

            List<LeaveRequest> leaveRequests;
            List<SickLeave> sickLeaves;
            if (LoggedInUserRole == Role.Manager)
            {
                leaveRequests = Context.LeaveRequests
                    .Where(p => (selectedStatus == null || p.Status == selectedStatus) && p.EndDate > DateTime.Now)
                    .OrderBy(p => p.StartDate)
                    .ToList();
                sickLeaves = Context.SickLeaves.OrderByDescending(p => p.Date).ToList();
            }
            else
            {
                leaveRequests = Context.LeaveRequests
                    .Where(e => e.Employee == _loggedInEmployee)
                    .Where(p => selectedStatus == null || p.Status == selectedStatus)
                    .OrderBy(p => p.StartDate)
                    .ToList();
                sickLeaves = Context.SickLeaves.Where(e => e.Employee == _loggedInEmployee).OrderByDescending(p => p.Date).ToList();
            }

            if (orderBy == SortOrder.Descending)
            {
                leaveRequests.Reverse();
            }

            int currentPageNumber = page ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)leaveRequests.Count / PageSize));

            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;

            ViewBag.OrderBy = orderBy ?? SortOrder.Ascending;

            var viewModel = new LeaveRequestViewModel
            {
                LeaveRequestsForPage = leaveRequests
                    .Skip((currentPageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                SelectedStatus = selectedStatus,
                SickLeaves = sickLeaves.Skip((currentPageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                LoggedInEmployee = _loggedInEmployee,
                AllEmployees = Context.Employees.Skip(1).ToList()
            };

            return View(viewModel);
        }

        public IActionResult LeaveRequest(int? id)
        {
            _loggedInEmployee = GetLoggedInEmployee();

            LeaveRequest leaveRequest;
            int amountOfLeaveHours = 0;
            if (id == null)
            {
                leaveRequest = null;
                var leaveRequests = Context.LeaveRequests
                    .Where(e => e.Employee == _loggedInEmployee)
                    .Where(s => s.Status == Status.Geaccepteerd || s.Status == Status.Aangevraagd)
                    .ToList();
                var usedLeaveHoursThisYear = CalculateLeaveHoursByYearForAllRequests(leaveRequests);
                amountOfLeaveHours = usedLeaveHoursThisYear[DateTime.Now.Year];
            }
            else
            {
                leaveRequest = Context.LeaveRequests
                          .Include(lr => lr.Employee)
                          .FirstOrDefault(lr => lr.Id == id);
            }

            var viewModel = new LeaveRequestDetailViewModel
            {
                LoggedInEmployee = _loggedInEmployee,
                LeaveRequest = leaveRequest,
                AmountOfUsedLeaveRequestHours = amountOfLeaveHours
            };
            return View(viewModel);
        }

        public IActionResult CreateLeaveRequest(LeaveRequest request)
        {
            _loggedInEmployee = GetLoggedInEmployee();
            request.Employee = _loggedInEmployee;
            var employeeLeaveRequests = Context.LeaveRequests
                    .Where(e => e.Employee == _loggedInEmployee)
                    .Where(s => s.Status == Status.Geaccepteerd || s.Status == Status.Aangevraagd)
                    .ToList();

            var amountOfLeaveHoursYear = CalculateLeaveHoursByYearForAllRequests(employeeLeaveRequests);
            var amountOfLeaveHoursLeaveRequest = CalculateLeaveHoursByYear(request.StartDate, request.EndDate);

            var startYearHours = amountOfLeaveHoursLeaveRequest.GetValueOrDefault(request.StartDate.Year, 0)
                     + amountOfLeaveHoursYear.GetValueOrDefault(request.StartDate.Year, 0);
            var endYearHours = amountOfLeaveHoursLeaveRequest.GetValueOrDefault(request.EndDate.Year, 0)
                               + amountOfLeaveHoursYear.GetValueOrDefault(request.EndDate.Year, 0);

            // validation 
            if (request.StartDate < DateTime.Now)
            {
                return NotifyErrorAndRedirect("Je kunt geen verlofaanvraag in het verleden doen.", "Index");
            }
            if (request.StartDate > request.EndDate)
            {
                return NotifyErrorAndRedirect("De startdatum moet voor of op de einddatum vallen.", "LeaveRequest");
            }

            if (_loggedInEmployee.LeaveHours < startYearHours || _loggedInEmployee.LeaveHours < endYearHours)
            {
                return NotifyErrorAndRedirect("Je hebt niet genoeg verlofuren om een verlofaanvraag te doen.", "Index");
            }
            if (LoggedInUserRole == Role.Manager)
            {
                return NotifyErrorAndRedirect("Je kan geen verlofaanvraag doen als manager", "Index");
            }

            bool hasOverlappingLeaveRequest = Context.LeaveRequests
                .Any(lr =>
                    lr.Employee == _loggedInEmployee &&
                    lr.StartDate <= request.EndDate &&
                    lr.EndDate >= request.StartDate);

            if (hasOverlappingLeaveRequest)
            {
                return NotifyErrorAndRedirect("Je hebt al een overlappende verlofaanvraag", "Index");
            }

            using var transaction = Context.Database.BeginTransaction();

            try
            {
                Context.LeaveRequests.Add(request);

                var manager = Context.Employees.Find(1);
                var notification = new Notification
                {
                    Employee = manager,
                    Title = "Nieuwe verlofaanvraag",
                    Description = "Er is een nieuwe verlofaanvraag om te beoordelen",
                    SentAt = DateTime.Now,
                    HasBeenRead = false
                };
                Context.Notifications.Add(notification);
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De verlofaanvraag is opgeslagen.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de verlofaanvraag", "Index");
            }
        }

        public IActionResult AssessLeaveRequest(Status status, int leaveRequestId)
        {
            if (leaveRequestId != null && LoggedInUserRole == Role.Manager)
            {
                using var transaction = Context.Database.BeginTransaction();
                try
                {
                    var leaveRequest = Context.LeaveRequests
                                  .Include(lr => lr.Employee)
                                  .FirstOrDefault(lr => lr.Id == leaveRequestId);
                    var leaveRequestEmployee = Context.Employees.Find(leaveRequest.Employee.EmployeeNumber);
                    leaveRequest.Status = status;

                    var notification = new Notification
                    {
                        Employee = leaveRequestEmployee,
                        Title = "Nieuwe verlofaanvraag status",
                        Description = "Je verlofaanvraag is beoordeeld",
                        SentAt = DateTime.Now,
                        HasBeenRead = false
                    };
                    Context.Notifications.Add(notification);

                    Context.SaveChanges();
                    transaction.Commit();
                    return NotifySuccessAndRedirect("De nieuwe verlofaanvraag status is opgeslagen.", "Index");
                }
                catch
                {
                    transaction.Rollback();
                    return NotifyErrorAndRedirect("Er is iets misgegaan", "Index");
                }
            }
            else
            {
                return NotifyErrorAndRedirect("Er is iets misgegaan", "Index");
            }
        }

        public Dictionary<int, int> CalculateLeaveHoursByYearForAllRequests(
                List<LeaveRequest> leaveRequests)
        {
            // Dictionary to store total leave hours used per year
            var totalLeaveHoursByYear = new Dictionary<int, int>();

            foreach (var leaveRequest in leaveRequests)
            {
                // Calculate leave hours by year for the current leave request
                var leaveHoursByYear = CalculateLeaveHoursByYear(leaveRequest.StartDate, leaveRequest.EndDate);

                // Aggregate the leave hours into the total dictionary
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

        // Splits leave hours for a single leave request across years
        public Dictionary<int, int> CalculateLeaveHoursByYear(DateTime start, DateTime end)
        {
            var leaveHoursByYear = new Dictionary<int, int>();

            // Ensure the start date is not after the end date
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

                // Calculate the difference between start and end dates
                TimeSpan difference = effectiveEnd - effectiveStart;
                int totalDays = difference.Days;

                int amountOfLeaveHours = 0;

                // Calculate leave hours based on the given logic
                if (totalDays > 0)
                {
                    amountOfLeaveHours = totalDays * 8;
                }
                else
                {
                    // Handle partial days
                    if (difference.Hours > 8)
                    {
                        amountOfLeaveHours = 8;
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

        private Employee GetLoggedInEmployee()
        {
            var username = User?.Identity?.Name;
            _loggedInEmployee = Context.Employees
            .FirstOrDefault(e => e.FirstName.Equals(username));

            return _loggedInEmployee;
        }


        [HttpPost]
        public IActionResult CreateSickLeave(SickLeave sickLeave)
        {
            if (sickLeave.EmployeeNumber == 0)
            {
                return NotifyErrorAndRedirect("Er was geen werknemer geselecteerd", "Index");
            }
            if (sickLeave.Date.ToDateTime(TimeOnly.MinValue) < DateTime.Now.Date)
            {
                return NotifyErrorAndRedirect("Je kunt geen ziekmelding in het verleden doen.", "Index");
            }
            if (sickLeave.Date.ToDateTime(TimeOnly.MinValue) > DateTime.Now.Date.AddDays(1))
            {
                return NotifyErrorAndRedirect("Je kunt je niet verder dan 1 dag in de toekomst ziekmelden.", "Index");
            }

            bool hasOverlappingSickLeave = Context.SickLeaves
                .Any(sl =>
                    sl.EmployeeNumber == sickLeave.EmployeeNumber &&
                    sl.Date == sickLeave.Date);

            if (hasOverlappingSickLeave)
            {
                return NotifyErrorAndRedirect("Werknemer is al ziekgemeld op deze dag", "Index");
            }

            Employee employee = Context.Employees.FirstOrDefault(e => e.EmployeeNumber == sickLeave.EmployeeNumber);
            sickLeave.Employee = employee;

            List<Shift> EmployeeShifts = Context.Shifts
                                    .Where(s => s.Employee != null &&
                                                s.Employee.EmployeeNumber == sickLeave.EmployeeNumber &&
                                                s.Start.Date == sickLeave.Date.ToDateTime(TimeOnly.MinValue).Date) // checks if the shift starts on the sick leave date
                                    .ToList();

            using var transaction = Context.Database.BeginTransaction();

            try
            {
                Context.SickLeaves.Add(sickLeave);
                foreach (Shift shift in EmployeeShifts)
                {
                    ShiftTakeOver existingShiftTakeOver = Context.ShiftTakeOvers.FirstOrDefault(st => st.ShiftId == shift.Id);
                    if (existingShiftTakeOver == null)
                    {
                        var shiftTakeOver = new ShiftTakeOver
                        {
                            ShiftId = shift.Id,
                            Shift = shift,
                            Status = Status.Aangevraagd
                        };
                        shift.ShiftTakeOver = shiftTakeOver;
                        Context.ShiftTakeOvers.Add(shiftTakeOver);
                    }
                    shift.Employee = null;

                }
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De ziekmelding is opgeslagen.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de ziekmelding", "Index");
            }
        }


    }
}
