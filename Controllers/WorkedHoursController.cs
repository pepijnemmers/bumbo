using AspNetCoreHero.ToastNotification.Notyf;
using BumboApp.Helpers;
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
        var workedHoursHelper = new WorkedHoursHelper();

        Employee? selectedEmployee = null;
        if (!string.IsNullOrEmpty(employee) && int.TryParse(employee, out int employeeNumber))
        {
            selectedEmployee = Context.Employees.Find(employeeNumber);
        }

        // Get all employees for the dropdown
        var allEmployees = Context.Employees.ToList();

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

                // Determine if there's a difference
                TimeOnly? plannedStart = TimeOnly.Parse(x.Shift.Start.ToString("HH:mm"));
                TimeOnly? plannedEnd = TimeOnly.Parse(x.Shift.End.ToString("HH:mm"));
                bool hasHourDifference = workedHoursHelper.HasHourDifference(x.WorkedHour?.StartTime, x.WorkedHour?.EndTime, plannedStart, plannedEnd);

                return new WorkedHourViewModel
                {
                    Id = x.WorkedHour?.Id,
                    Employee = x.Shift.Employee,
                    StartTime = x.WorkedHour?.StartTime, 
                    EndTime = x.WorkedHour?.EndTime,     
                    BreaksDuration = breakDuration,
                    TotalWorkedTime = totalWorkedTime,
                    Status = x.WorkedHour?.Status,
                    PlannedShift = x.Shift.Start.ToString("HH:mm") + " - " + x.Shift.End.ToString("HH:mm"),
                    IsFuture = DateOnly.FromDateTime(x.Shift.Start) <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(x.Shift.Start).Month == DateOnly.FromDateTime(DateTime.Now).Month,
                    HasHourDifference = hasHourDifference
                };
            })
            .OrderBy(e => e.StartTime)
            .ToList();

        // Apply employee filter
        if (selectedEmployee != null)
        {
            combinedHours = combinedHours
                .Where(wh => wh.Employee != null && wh.Employee.EmployeeNumber == selectedEmployee.EmployeeNumber)
                .ToList();
        }

        // Apply hour difference filter
        if (hours != null)
        {
            combinedHours = combinedHours
                .Where(wh =>
                {
                    TimeOnly? plannedStart = null;
                    TimeOnly? plannedEnd = null;

                    if (wh.PlannedShift != null)
                    {
                        var plannedTimes = wh.PlannedShift.Split(" - ");
                        plannedStart = TimeOnly.Parse(plannedTimes[0]);
                        plannedEnd = TimeOnly.Parse(plannedTimes[1]);
                    }

                    return workedHoursHelper.HasHourDifference(wh.StartTime, wh.EndTime, plannedStart, plannedEnd) == true;
                })
                .ToList();
        }

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
            Employee = selectedEmployee,
            Hours = hours,
            WorkedHours = workedHoursForPage
        };

        ViewBag.AllEmployees = allEmployees;

        return View(pageViewModel);
    }

    [HttpPost]
    public IActionResult MakeFinal(DateOnly date)
    {
        List<WorkedHour> workedHours = Context.WorkedHours
            .Where(e => e.DateOnly == date)
           .ToList();

        try
        {
            foreach (var workedHour in workedHours)
            {
                if (workedHour.EndTime == null)
                {
                    NotifyService.Error("Niet alle uren zijn volledig");
                    return RedirectToAction("Index", new { startDate = date });
                }

                workedHour.Status = HourStatus.Final;
            }

            Context.SaveChanges();
            NotifyService.Success("De uren zijn definitief gemaakt");
        }
        catch
        {
            NotifyService.Error("Er is iets fout gegaan.");
        }

        return RedirectToAction("Index", new { startDate = date });
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