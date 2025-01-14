using BumboApp.Models;

namespace BumboApp.ViewModels;

public class WorkedHoursUpdateViewModel
{
    public required int Id { get; set; }
    public required DateOnly Date { get; set; }
    public required Employee Employee { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly? EndTime { get; set; }
    public required TimeSpan BreakDuration { get; set; }
    public required TimeSpan WorkDuration { get; set; }
}