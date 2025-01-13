using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class CreateUpdateShiftViewModel
    {
        public Shift? Shift { get; set; }
        public required List<Department> Departments { get; set; }
        public required List<Employee> Employees { get; set; }
        
        public DateOnly? SelectedDate { get; set; }
        public TimeOnly? SelectedStartHour { get; set; }
        public Department? SelectedDepartment { get; set; }
        public OpeningHour? OpeningHour { get; set; }
    }

}
