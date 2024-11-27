using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        Employee LoggedInEmployee;
        public IActionResult Index()
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

            ViewData["LoggedInUser"] = LoggedInUser;
            return View(leaveRequests);
        }

        public IActionResult LeaveRequest()
        {
            return View(LoggedInUser);
        }

        public IActionResult CreateLeaveRequest(LeaveRequest request)
        {
            LoggedInEmployee = Context.Employees.Find(LoggedInUser.Id);
            request.Employee = LoggedInEmployee;

            // validation 
            if (request.StartDate < DateTime.Now)
                return NotifyErrorAndRedirect("Je kunt geen verlofaanvraag in het verleden doen.", "Index");

            if (request.StartDate > request.EndDate)
            {
                return NotifyErrorAndRedirect("De startdatum moet voor of op de einddatum vallen.", "LeaveRequest");
            }

            using var transaction = Context.Database.BeginTransaction();

            try
            {
                Context.LeaveRequests.Add(request);
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
