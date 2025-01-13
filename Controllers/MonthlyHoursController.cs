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
    public IActionResult Differing()
    {
        return View();
    }
}