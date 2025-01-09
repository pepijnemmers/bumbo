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
            selectedMonth = DateTime.Now.Month - 1; // current month
        }

        if (selectedYear > DateTime.Now.Year || selectedYear == null)
        {
            selectedYear = DateTime.Now.Year; // current year
        }
        var plannedShifts = Context.Shifts.Where(s => s.Start.Month == selectedMonth && s.Start.Year == selectedYear).ToList();
        var workedHours = Context.WorkedHours.Where(wh => wh.DateOnly.Month == selectedMonth && wh.DateOnly.Year == selectedYear).ToList();

        var amountOfPlannedHours = 0;
        var amountOfWorkedHours = 0;

        var viewModel = new MonthlyHoursViewModel
        {
            SelectedMonth = (int)selectedMonth,
            SelectedYear = (int)selectedYear,
            PlannedHours = amountOfPlannedHours,
            WorkedHours = amountOfWorkedHours,
            Difference = amountOfWorkedHours - amountOfPlannedHours
        };
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult Differing()
    {
        return View();
    }
}