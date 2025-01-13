using BumboApp.Models;

namespace BumboApp.ViewModels;

public class ScheduleViewModel
{
    public required Role Role { get; set; }
    
    public required bool ViewIsConcept { get; set; }
    public required DateOnly SelectedStartDate { get; set; }
    public required int WeekNumber { get; set; }
    
    public required WeekPrognosis? WeekPrognosis { get; set; }
    public required List<OpeningHour> OpeningHours { get; set; }
    public required List<Employee> Employees { get; set; }
    public required List<Shift> Shifts { get; set; }
    
    public required Employee? SelectedEmployee { get; set; }
    public required bool IsDayView { get; set; }
}