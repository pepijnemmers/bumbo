using BumboApp.Models;

namespace BumboApp.ViewModels;

public class HourRegistrationViewModel
{
    // hour registration
    public required TimeOnly? ClockInStart { get; set; }
    public required TimeOnly? BreakStart { get; set; }
    public required TimeSpan? TakenBreakDuration { get; set; }
    
    // hour history
    public required DateOnly SelectedDate { get; set; }
    public required List<WorkedHour>? WeekWorkedHours { get; set; }
}