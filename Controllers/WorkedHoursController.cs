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
    public IActionResult Update(int id)
    {
        // Get worked hour by id
        var workedHour = Context.WorkedHours
            .Include(wh => wh.Employee)
            .Include(wh => wh.Breaks)
            .FirstOrDefault(wh => wh.Id == id);
        if (workedHour == null)
            return NotifyErrorAndRedirect("Gewerkte uren niet gevonden", "Index");
        
        // Validate
        if (workedHour.EndTime == null || (workedHour.Breaks != null && workedHour.Breaks.Any(b => b.EndTime == null)))
            return NotifyErrorAndRedirect("Gewerkte uren/pauzes zijn nog bezig", "Index");
        
        // Get break duration
        var breakDuration = workedHour.Breaks
        ?.Where(b => b.EndTime != null)
        .Sum(b => (b.EndTime - b.StartTime)?.Ticks ?? 0);
        var breakDurationAsTime = breakDuration.HasValue ? TimeSpan.FromTicks(breakDuration.Value) : TimeSpan.Zero;
        
        // Get worked hours
        var workDuration = workedHour.EndTime - workedHour.StartTime - breakDurationAsTime;

        var viewModel = new WorkedHoursUpdateViewModel()
        {
            Id = workedHour.Id,
            Date = workedHour.DateOnly,
            Employee = workedHour.Employee,
            StartTime = workedHour.StartTime,
            EndTime = workedHour.EndTime,
            BreakDuration = breakDurationAsTime,
            WorkDuration = workDuration ?? TimeSpan.Zero
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Update(int id, TimeOnly startTime, TimeOnly endTime, TimeSpan breakTime)
    {
        // Get worked hour by id
        var workedHour = Context.WorkedHours
            .Include(wh => wh.Employee)
            .Include(wh => wh.Breaks)
            .FirstOrDefault(wh => wh.Id == id);
        
        // Validation
        if (!ModelState.IsValid)
            return NotifyErrorAndRedirect("Er is iets misgegaan bij het updaten van de gewerkte uren", "Index");
        if (workedHour == null)
            return NotifyErrorAndRedirect("Gewerkte uren niet gevonden", "Index");
        if (endTime < startTime)
            return NotifyErrorAndRedirect("Eindtijd kan niet voor de starttijd liggen", "Index");
        if (endTime - startTime - breakTime < new TimeSpan(0, 1, 0))
            return NotifyErrorAndRedirect("Gewerkte uren kunnen niet korter zijn dan 1 minuut", "Index");
        
        
        // Get break duration
        var breakDuration = workedHour!.Breaks
            ?.Where(b => b.EndTime != null)
            .Sum(b => (b.EndTime - b.StartTime)?.Ticks ?? 0);
        var breakDurationAsTime = breakDuration.HasValue ? TimeSpan.FromTicks(breakDuration.Value) : TimeSpan.Zero;
        
        // Check if break is changed and update
        if (breakDurationAsTime != breakTime)
        {
            try
            {
                if (workedHour.Breaks == null) throw new Exception();
                    
                Context.Breaks.RemoveRange(
                    workedHour.Breaks
                        .Where(b => b.WorkedHour == workedHour));
                
                Context.Breaks.Add(new Break()
                {
                    StartTime = workedHour.StartTime,
                    EndTime = workedHour.StartTime.Add(breakTime),
                    WorkedHour = workedHour
                });
            }
            catch
            {
                NotifyService.Error("Er is iets misgegaan bij het updaten van de pauze");
                return RedirectToAction("Index", new { startDate = workedHour.DateOnly });
            }
        }
        
        // Update worked hour
        try
        {
            workedHour.StartTime = startTime;
            workedHour.EndTime = endTime;
            Context.WorkedHours.Update(workedHour);
            Context.SaveChanges();
            NotifyService.Success("Gewerkte uren zijn succesvol geüpdatet");
        }
        catch
        {
            NotifyService.Error("Er is iets misgegaan bij het updaten van de gewerkte uren");
        }
        return RedirectToAction("Index", new { startDate = workedHour.DateOnly });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        try
        {
            var workedHour = Context.WorkedHours
                .Include(wh => wh.Employee)
                .Include(wh => wh.Breaks)
                .FirstOrDefault(wh => wh.Id == id);
            if (workedHour == null) throw new Exception();
            
            Context.WorkedHours.Remove(workedHour);
            Context.SaveChanges();
            NotifyService.Success("Gewerkte uren zijn succesvol verwijderd");
        }
        catch
        {
            NotifyService.Error("Er is iets misgegaan bij het verwijderen van de gewerkte uren");
        }
        return RedirectToAction("Index", new { startDate = DateOnly.FromDateTime(DateTime.Today) });
    }
}