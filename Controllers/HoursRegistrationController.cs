using BumboApp.Helpers;
using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers;

public class HoursRegistrationController : MainController
{
    private Employee? _employee;
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        CheckPageAccess(Role.Employee);
    }

    [HttpGet]
    public IActionResult Index(DateOnly? date)
    {
        // Get employee
        _employee = Context.Employees.FirstOrDefault(e => e.User.Id == LoggedInUserId);
        if (_employee == null) return NotifyErrorAndRedirect("Er is iets mis gegaan bij het laden van de urenregistratie pagina", "Index", "Dashboard");
        
        // Get worked hour when date is today
        var workedHour = Context.WorkedHours
            .Include(wh => wh.Breaks)
            .FirstOrDefault(wh => wh.Employee == _employee 
                                  && wh.DateOnly == DateOnly.FromDateTime(DateTime.Now)
                                  && wh.EndTime == null);
        
        // Get breaks from worked hour of today
        var currentBreak = workedHour?.Breaks?.FirstOrDefault(b => b.EndTime == null);
        var takenBreakDuration = workedHour?.Breaks
            ?.Where(b => b.EndTime != null)
            .Sum(b => (b.EndTime - b.StartTime)?.Ticks ?? 0);

        // Get date as monday
        var monday = DateConvertorHelper.GetMondayOfWeek(date ?? DateOnly.FromDateTime(DateTime.Now));
        
        // Get worked hours of the week
        var weekWorkedHours = Context.WorkedHours
            .Include(wh => wh.Breaks)
            .Where(wh => wh.Employee == _employee 
                         && wh.EndTime != null
                         && wh.DateOnly >= monday 
                         && wh.DateOnly <= monday.AddDays(6))
            .ToList();
        
        var viewModel = new HourRegistrationViewModel()
        {
            ClockInStart = workedHour?.StartTime,
            BreakStart = currentBreak?.StartTime,
            TakenBreakDuration = takenBreakDuration.HasValue ? TimeSpan.FromTicks(takenBreakDuration.Value) : null,
            
            SelectedDate = monday,
            WeekWorkedHours = weekWorkedHours
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult StartClock(DateOnly returnDate)
    {
        // Get employee
        _employee ??= Context.Employees.FirstOrDefault(e => e.User.Id == LoggedInUserId);
        if (_employee == null)
        {
            NotifyService.Error("Er is iets mis gegaan bij het inklokken");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Check if clocked in
        var currentWorkedHour = Context.WorkedHours
            .Include(wh => wh.Breaks)
            .FirstOrDefault(wh => wh.Employee == _employee 
                                  && wh.DateOnly == DateOnly.FromDateTime(DateTime.Now)
                                  && wh.EndTime == null);
        if (currentWorkedHour != null)
        {
            NotifyService.Error("Je bent al ingeklokt");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Clock the employee in
        try
        {
            var workedHour = new WorkedHour()
            {
                DateOnly = DateOnly.FromDateTime(DateTime.Now),
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                EndTime = null,
                Status = HourStatus.Concept,
                Employee = _employee,
                Breaks = null
            };
            Context.WorkedHours.Add(workedHour);
            Context.SaveChanges();
            
            NotifyService.Success("Je bent ingeklokt");
        }
        catch
        {
            NotifyService.Error("Er is iets mis gegaan bij het inklokken");
        }
        return RedirectToAction("Index", new {date = returnDate});
    }
    
    [HttpPost]
    public IActionResult EndClock(DateOnly returnDate)
    {
        // Get employee
        _employee ??= Context.Employees.FirstOrDefault(e => e.User.Id == LoggedInUserId);
        if (_employee == null)
        {
            NotifyService.Error("Er is iets mis gegaan bij het inklokken");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Check if clocked in
        var currentWorkedHour = Context.WorkedHours
            .Include(wh => wh.Breaks)
            .FirstOrDefault(wh => wh.Employee == _employee 
                                  && wh.DateOnly == DateOnly.FromDateTime(DateTime.Now)
                                  && wh.EndTime == null);
        if (currentWorkedHour == null)
        {
            NotifyService.Error("Je bent nog niet ingeklokt");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Clock the employee out
        try
        {
            currentWorkedHour.EndTime = TimeOnly.FromDateTime(DateTime.Now);
            Context.WorkedHours.Update(currentWorkedHour);
            Context.SaveChanges();
            
            NotifyService.Success("Je bent uitgeklokt");
        }
        catch
        {
            NotifyService.Error("Er is iets mis gegaan bij het uitklokken");
        }
        return RedirectToAction("Index", new {date = returnDate});
    }
    
    [HttpPost]
    public IActionResult StartPause(DateOnly returnDate)
    {
        // Get employee
        _employee ??= Context.Employees.FirstOrDefault(e => e.User.Id == LoggedInUserId);
        if (_employee == null)
        {
            NotifyService.Error("Er is iets mis gegaan bij het pauzeren");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Check if clocked in
        var currentWorkedHour = Context.WorkedHours
            .Include(wh => wh.Breaks)
            .FirstOrDefault(wh => wh.Employee == _employee 
                                  && wh.DateOnly == DateOnly.FromDateTime(DateTime.Now)
                                  && wh.EndTime == null);
        if (currentWorkedHour == null)
        {
            NotifyService.Error("Je bent nog niet ingeklokt");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Check if on break
        var currentBreak = currentWorkedHour.Breaks?.FirstOrDefault(b => b.EndTime == null);
        if (currentBreak != null)
        {
            NotifyService.Error("Je bent al aan het pauzeren");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Start the pause
        try
        {
            var newBreak = new Break()
            {
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                EndTime = null,
                WorkedHour = currentWorkedHour
            };
            Context.Breaks.Add(newBreak);
            Context.SaveChanges();
            
            NotifyService.Success("Je pauze is gestart");
        }
        catch
        {
            NotifyService.Error("Er is iets mis gegaan bij het pauzeren");
        }
        return RedirectToAction("Index", new {date = returnDate});
    }
    
    [HttpPost]
    public IActionResult EndPause(DateOnly returnDate)
    {
        // Get employee
        _employee ??= Context.Employees.FirstOrDefault(e => e.User.Id == LoggedInUserId);
        if (_employee == null)
        {
            NotifyService.Error("Er is iets mis gegaan bij het beëindigen van de pauze");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Check if clocked in
        var currentWorkedHour = Context.WorkedHours
            .Include(wh => wh.Breaks)
            .FirstOrDefault(wh => wh.Employee == _employee 
                                  && wh.DateOnly == DateOnly.FromDateTime(DateTime.Now)
                                  && wh.EndTime == null);
        if (currentWorkedHour == null)
        {
            NotifyService.Error("Je bent nog niet ingeklokt");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Check if on break
        var currentBreak = currentWorkedHour.Breaks?.FirstOrDefault(b => b.EndTime == null);
        if (currentBreak == null)
        {
            NotifyService.Error("Je bent nog niet aan het pauzeren");
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Check if break is not too short
        if (TimeOnly.FromDateTime(DateTime.Now) - currentBreak.StartTime < TimeSpan.FromMinutes(1))
        {
            NotifyService.Error("Let op! De pauze is niet opgeslagen! Je kan geen pauze van minder dan 1 minuut nemen.");

            try
            {
                Context.Breaks.Remove(currentBreak);
                Context.SaveChanges();
            }
            catch
            {
                NotifyService.Error("Er is iets mis gegaan");
            }
            return RedirectToAction("Index", new {date = returnDate});
        }
        
        // Start the pause
        try
        {
            currentBreak.EndTime = TimeOnly.FromDateTime(DateTime.Now);
            Context.Breaks.Update(currentBreak);
            Context.SaveChanges();
            
            NotifyService.Success("Je pauze is geëindigd");
        }
        catch
        {
            NotifyService.Error("Er is iets mis gegaan bij het beëindigen van de pauze");
        }
        return RedirectToAction("Index", new {date = returnDate});
    }
}