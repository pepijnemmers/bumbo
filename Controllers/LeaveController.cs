using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        Employee LoggedInEmployee;
        public IActionResult Index(int? page, SortOrder? orderBy = SortOrder.Ascending)
        {
            LoggedInEmployee = Context.Employees.Find(LoggedInUser.Id);

            List<LeaveRequest> leaveRequests;
            if (LoggedInUser.Role == Role.Manager)
            {
                leaveRequests = Context.LeaveRequests.OrderBy(p => p.StartDate).ToList();
            }
            else
            {
                leaveRequests = LoggedInEmployee.leaveRequests;
            }
            if (orderBy == SortOrder.Descending)
            {
                leaveRequests.Reverse();
            }

            int currentPageNumber = page ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)leaveRequests.Count / PageSize));

            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            List<LeaveRequest> leaveRequestsForPage =
                    leaveRequests
                    .Skip((currentPageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;

            ViewBag.OrderBy = orderBy ?? SortOrder.Ascending;

            ViewData["LoggedInUser"] = LoggedInUser;
            return View(leaveRequestsForPage);
        }

        public IActionResult LeaveRequest()
        {
            return View(LoggedInUser);
        }

        public IActionResult CreateLeaveRequest(LeaveRequest request)
        {
            LoggedInEmployee = Context.Employees.Find(LoggedInUser.Id);
            request.Employee = LoggedInEmployee;

            TimeSpan difference = request.EndDate - request.StartDate;
            int totalDays = difference.Days + 1;
            int amountOfLeaveHours = totalDays * 8;

            // validation 
            if (request.StartDate < DateTime.Now)
                return NotifyErrorAndRedirect("Je kunt geen verlofaanvraag in het verleden doen.", "Index");

            if (request.StartDate > request.EndDate)
            {
                return NotifyErrorAndRedirect("De startdatum moet voor of op de einddatum vallen.", "LeaveRequest");
            }
            if (LoggedInEmployee.LeaveHours < amountOfLeaveHours)
            {
                return NotifyErrorAndRedirect("Je hebt niet genoeg verlofuren om een verlofaanvraag te doen.", "Index");
            }

            using var transaction = Context.Database.BeginTransaction();

            try
            {
                Context.LeaveRequests.Add(request);
                LoggedInEmployee.LeaveHours = LoggedInEmployee.LeaveHours - amountOfLeaveHours;
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
    }
}
