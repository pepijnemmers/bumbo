using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        Employee LoggedInEmployee;
        public IActionResult Index(int? page, SortOrder? orderBy = SortOrder.Ascending)
        {
            var username = User?.Identity?.Name;
            LoggedInEmployee = Context.Employees
            .FirstOrDefault(e => e.FirstName.Equals(username));

            List<LeaveRequest> leaveRequests;
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "Manager")
            {
                leaveRequests = Context.LeaveRequests.OrderBy(p => p.StartDate).ToList();
            }
            else
            {
                leaveRequests = Context.LeaveRequests.Where(e => e.Employee == LoggedInEmployee).OrderBy(P => P.StartDate).ToList();
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

            ViewData["LoggedInUser"] = User;
            return View(leaveRequestsForPage);
        }

        public IActionResult LeaveRequest()
        {
            return View();
        }

        public IActionResult CreateLeaveRequest(LeaveRequest request)
        {
            var username = User?.Identity?.Name;
            LoggedInEmployee = Context.Employees
            .FirstOrDefault(e => e.FirstName.Equals(username));
            request.Employee = LoggedInEmployee;

            TimeSpan difference = request.EndDate - request.StartDate;
            int totalDays = difference.Days + 1;
            int amountOfLeaveHours = totalDays * 8;

            // validation 
            if (request.StartDate < DateTime.Now)
            {
                return NotifyErrorAndRedirect("Je kunt geen verlofaanvraag in het verleden doen.", "Index");
            }

            if (request.StartDate > request.EndDate)
            {
                return NotifyErrorAndRedirect("De startdatum moet voor of op de einddatum vallen.", "LeaveRequest");
            }
            if (LoggedInEmployee.LeaveHours < amountOfLeaveHours)
            {
                return NotifyErrorAndRedirect("Je hebt niet genoeg verlofuren om een verlofaanvraag te doen.", "Index");
            }
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "Manager")
            {
                return NotifyErrorAndRedirect("Je kan geen verlofaanvraag doen als manager", "Index");
            }

            bool hasOverlappingLeaveRequest = Context.LeaveRequests
                .Any(lr =>
                    lr.Employee == LoggedInEmployee &&
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
    }
}
