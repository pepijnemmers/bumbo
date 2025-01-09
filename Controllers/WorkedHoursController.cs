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
            .Select(x => new WorkedHourViewModel
            {
                Employee = x.Shift.Employee,
                StartTime = x.WorkedHour?.StartTime ?? null,
                EndTime = x.WorkedHour?.EndTime ?? null,
                //Breaks = x.WorkedHour?.Breaks != null && x.WorkedHour.Breaks.Any()
                //    ? x.WorkedHour.Breaks.Select(b => $"{b.StartTime} - {b.EndTime}").ToList()
                //    : new List<string> { "-" },
                //BreaksDuration = x.WorkedHour?.Breaks != null && x.WorkedHour.Breaks.Any()
                //    ? TimeSpan.FromTicks(x.WorkedHour.Breaks
                //    .Sum(b => (b.EndTime - b.StartTime).Ticks))  // Sum the durations in minutes
                //    : 0,
                BreaksDuration = x.WorkedHour?.Breaks != null && x.WorkedHour.Breaks.Any()
                    ? TimeSpan.FromTicks(x.WorkedHour.Breaks
                    .Sum(b => (b.EndTime.ToTimeSpan() - b.StartTime.ToTimeSpan()).Ticks))  // Sum the ticks for all breaks
                    : TimeSpan.Zero,
                Status = x.WorkedHour != null ? "Registered" : "Not Registered",
                PlannedShift = x.Shift,
                IsFuture = DateOnly.FromDateTime(x.Shift.Start) > DateOnly.FromDateTime(DateTime.Now)
            })
            .OrderBy(e => e.PlannedShift.Start)
            .ToList();

        //// Apply additional filters
        //if (!string.IsNullOrEmpty(employee))
        //{
        //    combinedHours = combinedHours
        //        .Where(wh => $"{wh.Employee.FirstName} {wh.Employee.LastName}".Contains(employee, StringComparison.OrdinalIgnoreCase))
        //        .ToList();
        //}

        //if (!string.IsNullOrEmpty(hours))
        //{
        //    combinedHours = combinedHours
        //        //.Where(wh => wh.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
        //        .ToList();
        //}

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