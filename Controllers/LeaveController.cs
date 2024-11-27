using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        Employee LoggedInEmployee; // de Employee die is ingelogd (afgeleid van LoggedInUser)
        public IActionResult Index()
        {
            List<LeaveRequest> leaveRequests;
            if (LoggedInUser.Role == Role.Manager)
            {
                leaveRequests = Context.LeaveRequests.ToList();
            }
            else
            {
                leaveRequests = LoggedInEmployee.leaveRequests;
            }
            return View(leaveRequests);
        }

        public IActionResult LeaveRequest()
        {
            return View(LoggedInEmployee);
        }

        public IActionResult CreateLeaveRequest(LeaveRequest request)
        {
            return NotifyErrorAndRedirect("Fail", "Index"); // test dingetje

            /* uncommenten als de LoggedInEmployee correct werkt
            using var transaction = Context.Database.BeginTransaction();

            try
            {
                Context.LeaveRequests.Add(request);

                Context.SaveChanges();
                transaction.Commit();
                LoggedInEmployee.leaveRequests.Add(request);
                return NotifySuccessAndRedirect("De verlofaanvraag is opgeslagen.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de verlofaanvraag", "Index");
            }
            */
        }
    }
}
