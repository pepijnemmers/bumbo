using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers;

public class WorkedHoursController : MainController
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        CheckPageAccess(Role.Manager);
    }

    [HttpGet]
    public IActionResult Index(int? page, DateOnly? startDate, string? employee, string? hours)
    {
        DateOnly selectedStartDate = startDate ?? DateOnly.FromDateTime(DateTime.Today);

        List <Shift> plannedShifts = Context.Shifts
            .Include(s => s.Employee)
            .Where(s => DateOnly.FromDateTime(s.Start) == selectedStartDate)
            .ToList();

        List<WorkedHour> workedHours = Context.WorkedHours
            .Include(e => e.Employee)
            .ThenInclude(e => e.Shifts)
            .Include(e => e.Breaks)
            .Where(e => e.DateOnly == selectedStartDate)
            .OrderBy(e => e.DateOnly)
            .ToList();

        // Combine worked hours and planned shifts
        var combinedHours = plannedShifts
            .Select(shift => new
            {
                Shift = shift,
                WorkedHour = workedHours.FirstOrDefault(wh =>
                (shift.Employee != null && wh.Employee.EmployeeNumber == shift.Employee.EmployeeNumber) &&
                wh.DateOnly == DateOnly.FromDateTime(shift.Start))
            })
            .Select(x =>
            {
                // Calculate breaks duration 
                var breakDuration = x.WorkedHour?.Breaks
                    ?.Where(b => b.EndTime != null)
                    .Sum(b => (b.EndTime - b.StartTime)?.Ticks ?? 0) is long ticks && ticks > 0
                    ? TimeSpan.FromTicks(ticks)
                    : TimeSpan.Zero;

                // Calculate total worked time 
                var totalWorkedTime = (x.WorkedHour?.StartTime != null && x.WorkedHour?.EndTime != null)
                    ? x.WorkedHour.EndTime - x.WorkedHour.StartTime - breakDuration
                    : (TimeSpan?)null;

                return new WorkedHourViewModel
                {
                    Employee = x.Shift.Employee,
                    StartTime = x.WorkedHour?.StartTime, 
                    EndTime = x.WorkedHour?.EndTime,     
                    BreaksDuration = breakDuration,
                    TotalWorkedTime = totalWorkedTime,
                    Status = x.WorkedHour?.Status.ToString(),
                    PlannedShift = x.Shift.Start.ToString("HH:mm") + " - " + x.Shift.End.ToString("HH:mm"),
                    IsFuture = DateOnly.FromDateTime(x.Shift.Start) <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(x.Shift.Start).Month == DateOnly.FromDateTime(DateTime.Now).Month,
                };
            })
            .OrderBy(e => e.StartTime)
            .ToList();

        //Pagination
        int currentPageNumber = page ?? DefaultPage;
        int maxPages = (int)(Math.Ceiling((decimal)workedHours.Count / PageSize));
        if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
        if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

        var workedHoursForPage =
            combinedHours
            .Skip((currentPageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewBag.PageNumber = currentPageNumber;
        ViewBag.PageSize = PageSize;
        ViewBag.MaxPages = maxPages;

        var pageViewModel = new WorkedHoursPageViewModel()
        {
            SelectedStartDate = selectedStartDate,
            Employee = employee,
            Hours = hours,
            WorkedHours = workedHoursForPage
        };

        return View(pageViewModel);
    }

    [HttpGet]
    public IActionResult Update()
    {
        return View();
    }
}