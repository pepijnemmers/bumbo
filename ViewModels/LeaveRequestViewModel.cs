using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class LeaveRequestViewModel
    {
        public required List<LeaveRequest> LeaveRequestsForPage { get; set; }
        public required Status? SelectedStatus { get; set; }

        public required List<SickLeave> SickLeaves { get; set; }

        public required Employee LoggedInEmployee { get; set; }
    }
}
