using Azure.Core;
using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        private Employee _loggedInEmployee;
        public IActionResult Index(int? page, SortOrder? orderBy = SortOrder.Ascending, Status? selectedStatus = null)
        {
            var username = User?.Identity?.Name;
            _loggedInEmployee = Context.Employees
            .FirstOrDefault(e => e.FirstName.Equals(username));

            List<LeaveRequest> leaveRequests;
            List<SickLeave> sickLeaves;
            if (LoggedInUserRole == Role.Manager)
            {
                leaveRequests = Context.LeaveRequests
                    .Where(p => selectedStatus == null || p.Status == selectedStatus)
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

        public IActionResult LeaveRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLeaveRequest(LeaveRequest request)
        {
            var username = User?.Identity?.Name;
            _loggedInEmployee = Context.Employees
            .FirstOrDefault(e => e.FirstName.Equals(username));
            request.Employee = _loggedInEmployee;

            TimeSpan difference = request.EndDate - request.StartDate;
            int totalDays = difference.Days;

            // calculate amount of leavehours
            int amountOfLeaveHours = 0;
            if (totalDays > 0)
            {
                amountOfLeaveHours = totalDays * 8;
            }
            else
            {
                if (difference.Hours > 8)
                {
                    amountOfLeaveHours = 8;
                }
                else
                {
                    amountOfLeaveHours = difference.Hours;
                }
            }
            
            // validation 
            if (request.StartDate < DateTime.Now)
            {
                return NotifyErrorAndRedirect("Je kunt geen verlofaanvraag in het verleden doen.", "Index");
            }
            if (request.StartDate > request.EndDate)
            {
                return NotifyErrorAndRedirect("De startdatum moet voor of op de einddatum vallen.", "LeaveRequest");
            }
            if (_loggedInEmployee.LeaveHours < amountOfLeaveHours)
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
                // TODO fix notification
                //var manager = Context.Employees.Find(1);
                //var notification = new Notification
                //{
                //    Employee = manager,
                //    Title = "Nieuwe verlofaanvraag",
                //    Description = "Er staat een nieuwe verlofaanvraag klaar om beoordeeld te worden.",
                //    SentAt = DateTime.Now,
                //    HasBeenRead = false
                //};
                //Context.Notifications.Add(notification);
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
