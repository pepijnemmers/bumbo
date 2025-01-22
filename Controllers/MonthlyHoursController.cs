using BumboApp.Helpers;
using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers;

public class MonthlyHoursController : MainController
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        CheckPageAccess(Role.Manager);
    }

    [HttpGet]
    public IActionResult Index(int? selectedMonth, int? selectedYear)
    {
        if (selectedMonth > 11 || selectedMonth == null)
        {
            selectedMonth = DateTime.Now.Month - 1;
        }

        if (selectedYear > DateTime.Now.Year || selectedYear == null)
        {
            selectedYear = DateTime.Now.Year;
        }

        var plannedShifts = Context.Shifts.Where(s => s.Start.Month == selectedMonth + 1 && s.Start.Year == selectedYear).ToList();
        var workedHours = Context.WorkedHours.Include(wh => wh.Breaks).Where(wh => wh.DateOnly.Month == selectedMonth + 1 && wh.DateOnly.Year == selectedYear).ToList();

        double amountOfPlannedHours = 0;
        double amountOfWorkedHours = 0;

        foreach (var shift in plannedShifts)
        {
            var timespan = shift.End - shift.Start;
            var breakTime = BreakCalculationHelper.CalculateRequiredBreak(shift.Start, shift.End);
            amountOfPlannedHours += timespan.TotalHours;
            amountOfPlannedHours -= breakTime.TotalHours;

        }

        foreach (var workedHour in workedHours)
        {
            if (workedHour.EndTime != null)
            {
                var timespan = workedHour.EndTime.Value - workedHour.StartTime;
                var breakDuration = workedHour?.Breaks
                    ?.Where(b => b.EndTime != null)
                    .Sum(b => (b.EndTime - b.StartTime)?.Ticks ?? 0) is long ticks && ticks > 0
                    ? TimeSpan.FromTicks(ticks)
                    : TimeSpan.Zero;
                amountOfWorkedHours += timespan.TotalHours;
                amountOfWorkedHours -= breakDuration.TotalHours;
            }
        }
        amountOfPlannedHours = Math.Round(amountOfPlannedHours, 2);
        amountOfWorkedHours = Math.Round(amountOfWorkedHours, 2);

        var viewModel = new MonthlyHoursViewModel
        {
            SelectedMonth = (int)selectedMonth,
            SelectedYear = (int)selectedYear,
            PlannedHours = amountOfPlannedHours,
            WorkedHours = amountOfWorkedHours,
            Difference = Math.Round(amountOfWorkedHours - amountOfPlannedHours, 2)
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Differing(int? page, int? selectedMonth, int? selectedYear, int? selectedEmployee)
    {
        var workedHoursHelper = new WorkedHoursHelper();

        if (selectedMonth > 11 || selectedMonth == null)
        {
            selectedMonth = DateTime.Now.Month - 1;
        }

        if (selectedYear > DateTime.Now.Year || selectedYear == null)
        {
            selectedYear = DateTime.Now.Year;
        }

        Employee? employee = null;
        employee = Context.Employees.Find(selectedEmployee);

        // Get all employees for the dropdown
        var allEmployees = Context.Employees.ToList();

        List<Shift> plannedShifts = Context.Shifts
            .Include(s => s.Employee)
            .Where(s => s.Start.Month == selectedMonth + 1 && s.Start.Year == selectedYear)
            .ToList();

        List<WorkedHour> workedHours = Context.WorkedHours
            .Include(e => e.Employee)
            .ThenInclude(e => e.Shifts)
            .Include(e => e.Breaks)
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
                    : null;

                // Determine if there's a difference
                TimeOnly? plannedStart = TimeOnly.Parse(x.Shift.Start.ToString("HH:mm"));
                TimeOnly? plannedEnd = TimeOnly.Parse(x.Shift.End.ToString("HH:mm"));
                bool hasHourDifference = workedHoursHelper.HasHourDifference(x.WorkedHour?.StartTime, x.WorkedHour?.EndTime, plannedStart, plannedEnd, breakDuration);
                var hourDifference = workedHoursHelper.HourDifference(x.WorkedHour?.StartTime, x.WorkedHour?.EndTime, plannedStart, plannedEnd, breakDuration);

                return new WorkedHourViewModel
                {
                    Id = x.WorkedHour?.Id,
                    Employee = x.Shift.Employee,
                    StartTime = x.WorkedHour?.StartTime,
                    EndTime = x.WorkedHour?.EndTime,
                    Date = DateOnly.FromDateTime(x.Shift.Start),
                    BreaksDuration = breakDuration,
                    TotalWorkedTime = totalWorkedTime,
                    Status = x.WorkedHour?.Status,
                    PlannedShift = x.Shift.Start.ToString("HH:mm") + " - " + x.Shift.End.ToString("HH:mm"),
                    IsFuture = DateOnly.FromDateTime(x.Shift.Start) <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(x.Shift.Start).Month == DateOnly.FromDateTime(DateTime.Now).Month,
                    HasHourDifference = hasHourDifference,
                    HourDifference = hourDifference
                };
            })
            .OrderBy(e => e.Date)
            .ToList();

        // Add worked hours without shifts
        var unmatchedWorkedHours = workedHours
            .Where(wh => wh.DateOnly.Year == selectedYear && wh.DateOnly.Month == selectedMonth)
            .Where(wh => !plannedShifts.Any(shift =>
                shift.Employee != null &&
                shift.Employee.EmployeeNumber == wh.Employee.EmployeeNumber &&
                DateOnly.FromDateTime(shift.Start) == wh.DateOnly))
            .Select(wh =>
            {
                // Calculate breaks duration
                var breakDuration = wh.Breaks
                    ?.Where(b => b.EndTime != null)
                    .Sum(b => (b.EndTime - b.StartTime)?.Ticks ?? 0) is long ticks && ticks > 0
                    ? TimeSpan.FromTicks(ticks)
                    : TimeSpan.Zero;

                // Calculate total worked time
                var totalWorkedTime = wh.EndTime != null
                    ? wh.EndTime - wh.StartTime - breakDuration
                    : null;

                return new WorkedHourViewModel
                {
                    Id = wh.Id,
                    Employee = wh.Employee,
                    StartTime = wh.StartTime,
                    EndTime = wh.EndTime,
                    Date = wh.DateOnly,
                    BreaksDuration = breakDuration,
                    TotalWorkedTime = totalWorkedTime,
                    Status = wh.Status,
                    PlannedShift = null,
                    IsFuture = wh.DateOnly <= DateOnly.FromDateTime(DateTime.Now) && wh.DateOnly.Month == DateOnly.FromDateTime(DateTime.Now).Month,
                    HasHourDifference = true,
                    HourDifference = workedHoursHelper.HourDifference(wh.StartTime, wh.EndTime, new TimeOnly(0, 0, 0), new TimeOnly(0, 0, 0), breakDuration)
                };
            })
            .ToList();

        // Combine both lists
        combinedHours.AddRange(unmatchedWorkedHours);

        // Sort the combined list
        combinedHours = combinedHours
            .OrderBy(e => e.Date)
            .ToList();

        // Apply employee filter
        if (employee != null)
        {
            combinedHours = combinedHours
                .Where(wh => wh.Employee != null && wh.Employee.EmployeeNumber == employee.EmployeeNumber)
                .ToList();
        }

        // Apply hour difference filter
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

                var breaksDuration = wh.BreaksDuration;

                return workedHoursHelper.HasHourDifference(wh.StartTime, wh.EndTime, plannedStart, plannedEnd, breaksDuration) == true;
            })
            .ToList();

        //Pagination
        int currentPageNumber = page ?? DefaultPage;
        int maxPages = (int)(Math.Ceiling((decimal)combinedHours.Count / PageSize));
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

        var pageViewModel = new DifferingMonthlyHoursViewModel
        {
            SelectedMonth = (int)selectedMonth,
            SelectedYear = (int)selectedYear,
            Employee = employee,
            WorkedHours = workedHoursForPage
        };

        ViewBag.AllEmployees = allEmployees;

        return View(pageViewModel);
    }
}