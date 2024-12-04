using BumboApp.Models;

namespace BumboApp.ViewModels;

public class NotificationViewModel
{
    public List<Notification>? NotificationsForPage { get; set; }
    public int TotalNotifications { get; set; }
    public int TotalUnreadNotifications { get; set; }
}