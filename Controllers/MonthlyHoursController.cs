using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
        var workedHours = Context.WorkedHours.Where(wh => wh.DateOnly.Month == selectedMonth + 1 && wh.DateOnly.Year == selectedYear).ToList();

        double amountOfPlannedHours = 0;
        double amountOfWorkedHours = 0;

        foreach (var shift in plannedShifts)
        {
            var timespan = shift.End - shift.Start;
            amountOfPlannedHours += timespan.TotalHours;
        }

        foreach (var workedHour in workedHours)
        {
            if (workedHour.EndTime != null)
            {
                var timespan = workedHour.EndTime.Value - workedHour.StartTime;
                amountOfWorkedHours += timespan.TotalHours;
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