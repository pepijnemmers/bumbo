using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BumboApp.Controllers
{
    public class ShiftsController : MainController
    {
        public IActionResult Create(DateOnly? date, TimeOnly? startTime, TimeOnly? endTime )
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        public IActionResult Update()
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        public IActionResult MyShifts()
        {
            CheckPageAccess(Role.Employee);

            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) { return View(); }

            // Fetch the employee associated with the logged-in user
            var employee = Context.Employees
                .Include(e => e.Shifts)
                .FirstOrDefault(e => e.User.Id == userId);

            if (employee == null) { return View(); }

            var today = DateTime.Today;
            var now = DateTime.Now;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday); // Start of the week                                                                               // If today is Sunday, we want the week to start from the previous Monday
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                startOfWeek = startOfWeek.AddDays(-7); // Move back to the previous Monday
            }
            var endOfWeek = startOfWeek.AddDays(6); // End of the week

            // Filter shifts for the current week (shifts left this week)
            var shiftsThisWeek = employee.Shifts
                .Where(s => s.Start.Date >= today && s.Start.Date <= endOfWeek && s.IsFinal && s.Start > now)
                .OrderBy(s => s.Start)
                .ToList();

            // Filter shifts for the rest of the month (after this week)
            var endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)); // Last day of the current month
            var shiftsThisMonth = employee.Shifts
                .Where(s => s.Start.Date > endOfWeek && s.Start.Date <= endOfMonth && s.IsFinal)
                .OrderBy(s => s.Start)
                .ToList();

            // Filter shifts for later dates (after the current month)
            var shiftsLater = employee.Shifts
                .Where(s => s.Start.Date > endOfMonth && s.IsFinal)
                .OrderBy(s => s.Start)
                .ToList();

            var takenOverShiftIds = Context.ShiftTakeOvers
                .Select(st => st.ShiftId)
                .ToList();

            var takenOverShiftsThisWeek = shiftsThisWeek
                .Where(s => takenOverShiftIds.Contains(s.Id))
                .ToList();

            var takenOverShiftsThisMonth = shiftsThisMonth
                .Where(s => takenOverShiftIds.Contains(s.Id))
                .ToList();

            var takenOverShiftsLater = shiftsLater
                .Where(s => takenOverShiftIds.Contains(s.Id))
                .ToList();

            // Group the shifts
            ViewBag.StartOfWeek = startOfWeek;
            ViewBag.EndOfWeek = endOfWeek;
            ViewBag.EndOfMonth = endOfMonth;

            ViewBag.ShiftsThisWeek = shiftsThisWeek;
            ViewBag.ShiftsThisMonth = shiftsThisMonth;
            ViewBag.ShiftsLater = shiftsLater;

            ViewBag.TakenOverShiftsThisWeek = takenOverShiftsThisWeek;
            ViewBag.TakenOverShiftsThisMonth = takenOverShiftsThisMonth;
            ViewBag.TakenOverShiftsLater = takenOverShiftsLater;
            return View();
        }
    }
}
