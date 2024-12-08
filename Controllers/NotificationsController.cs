using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BumboApp.Controllers
{
    public class NotificationsController : MainController
    {
        public IActionResult Index(int? page)
        {
            var notifications = Context.Notifications
                .Where(n => n.Employee.User != null && n.Employee.User.Id == LoggedInUserId)
                .OrderByDescending(n => n.SentAt)
                .ToList();
            
            int currentPageNumber = page ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)notifications.Count / PageSize));
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }
            
            var notificationsForPage = 
                notifications
                    .Skip((currentPageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;

            var viewModel = new NotificationViewModel()
            {
                NotificationsForPage = notificationsForPage,
                TotalNotifications = notifications.Count,
                TotalUnreadNotifications = notifications.Count(n => !n.HasBeenRead)
            };
            return View(viewModel);
        }

        public IActionResult GoToActionUrl(int id)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het openen van de melding.", "Index");
            }
            return MarkAsRead(id, true);
        }

        public IActionResult MarkAsRead(int id, bool? fromGoToActionUrl = false)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het markeren van de melding als gelezen.", "Index");
            }
            
            var notification = Context.Notifications
                .FirstOrDefault(n => n.Id == id 
                                     && n.Employee.User != null && n.Employee.User.Id == LoggedInUserId);
            if (notification == null)
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het markeren van de melding als gelezen.", "Index");
            }

            try
            {
                notification.HasBeenRead = true;
                Context.SaveChanges();
                if (fromGoToActionUrl == true && !string.IsNullOrEmpty(notification.ActionUrl))
                {
                    return Redirect(notification.ActionUrl);
                }
                return NotifySuccessAndRedirect("De melding is gemarkeerd als gelezen.", "Index");
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het markeren van de melding als gelezen.", "Index");
            }
        }
        
        public IActionResult MarkAllAsRead()
        {
            try
            {
                var notifications = Context.Notifications
                    .Where(n => n.Employee.User != null && n.Employee.User.Id == LoggedInUserId)
                    .ToList();

                foreach (var notification in notifications)
                {
                    notification.HasBeenRead = true;
                }
                Context.SaveChanges();
                return NotifySuccessAndRedirect("Alle meldingen zijn gemarkeerd als gelezen.", "Index");
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het markeren van de meldingen als gelezen.", "Index");
            }
        }

        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het verwijderen van de melding.", "Index");
            }
            
            var notification = Context.Notifications
                .FirstOrDefault(n => n.Id == id 
                                     && n.Employee.User != null && n.Employee.User.Id == LoggedInUserId);
            if (notification == null)
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het verwijderen van de melding.", "Index");
            }
            
            try
            {
                Context.Notifications.Remove(notification);
                Context.SaveChanges();
                return NotifySuccessAndRedirect("De melding is verwijderd.", "Index");
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het verwijderen van de melding.", "Index");
            }
        }
    }
}
