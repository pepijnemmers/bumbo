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
    public IActionResult Index(int? page, DateOnly? startDate)
    {
        DateOnly selectedStartDate = startDate ?? DateOnly.FromDateTime(DateTime.Today);

        List <Shift> plannedShifts = Context.Shifts
            .Include(s => s.Employee)
            .Where(s => DateOnly.FromDateTime(s.Start.Date) == selectedStartDate)
            .ToList();

        List<WorkedHour> workedHours = Context.WorkedHours
            .Include(e => e.Employee)
            .ThenInclude(e => e.Shifts)
            .Include(e => e.Breaks)
            .Where(wh => wh.DateOnly == selectedStartDate)
            .OrderBy(e => e.DateOnly)
            .ToList();

        // Combine worked hours and planned shifts
        var combinedHours = plannedShifts
            .Select(shift => new
            {
                Shift = shift,
                WorkedHour = workedHours.FirstOrDefault(wh => 
                shift.Employee != null &&
                wh.Employee.EmployeeNumber == shift.Employee.EmployeeNumber &&
                wh.DateOnly == DateOnly.FromDateTime(shift.Start.Date))
            })
            .Select(x => new WorkedHourViewModel
            {
                Employee = x.Shift.Employee,
                StartTime = x.WorkedHour?.StartTime ?? null,
                EndTime = x.WorkedHour?.EndTime ?? null,
                Breaks = x.WorkedHour?.Breaks != null && x.WorkedHour.Breaks.Any() ? string.Join(", ", x.WorkedHour.Breaks.Select(b => $"{b.StartTime} - {b.EndTime}")) : "Not Registered",
                Status = x.WorkedHour != null ? "Registered" : "Not Registered",
                PlannedShift = x.Shift,
                IsFuture = DateOnly.FromDateTime(x.Shift.Start.Date) > DateOnly.FromDateTime(DateTime.Now.Date),
                SelectedStartDate = selectedStartDate
            })
            .OrderBy(e => e.PlannedShift.Start)
            .ToList();

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

        return View(workedHoursForPage);
    }

    [HttpGet]
    public IActionResult Update()
    {
        return View();
    }
}