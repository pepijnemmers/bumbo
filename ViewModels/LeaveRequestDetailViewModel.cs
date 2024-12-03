using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class LeaveRequestDetailViewModel
    {
        public required Employee LoggedInEmployee { get; set; }
        public LeaveRequest LeaveRequest { get; set; }
        public int AmountOfUsedLeaveRequestHours { get; set; }

    }
}
