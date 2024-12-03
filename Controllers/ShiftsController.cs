using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ShiftsController : MainController
    {
        [HttpGet]
        public IActionResult Create(string? date, int? startHour, string? department)
        {
            CheckPageAccess(Role.Manager);

            var viewModel = new CreateUpdateShiftViewModel
            {
                Departments = Enum.GetValues<Department>().ToList(),
                Employees = Context.Employees
                    .ToList(),
                
                SelectedDate = date != null ? DateOnly.FromDateTime(DateTime.Parse(date)) : null,
                SelectedStartHour = startHour != null ? new TimeOnly(startHour.Value, 0) : null,
                SelectedDepartment = department != null ? Enum.Parse<Department>(department) : null
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(DateOnly? date, TimeOnly? start, TimeOnly? end, Department? department, int? employee, bool? isFinal)
        {
            // validation
            if (date == null || start == null || end == null || department == null || employee == null || isFinal == null)
            {
                return NotifyErrorAndRedirect("Niet alle velden zijn ingevuld", "Create");
            }
            if (date < DateOnly.FromDateTime(DateTime.Today))
            {
                return NotifyErrorAndRedirect("De datum moet in de toekomst zijn", "Create");
            }
            if (start >= end)
            {
                return NotifyErrorAndRedirect("De eindtijd moet na de starttijd zijn", "Create");
            }
            if (Enum.TryParse<Department>(department.ToString(), out var dep) == false)
            {
                return NotifyErrorAndRedirect("De afdeling is niet geldig", "Create");
            }
            
            // add to database
            try
            {
                var startToInsert = new DateTime(date.Value, start.Value);
                var endToInsert = new DateTime(date.Value, end.Value);
                    
                // check if employee already has a shift at that time
                if (employee != 0 && 
                    Context.Shifts
                        .Any(s => s.Employee != null
                                  && s.Employee.EmployeeNumber == employee
                                  && s.Start < endToInsert
                                  && s.End > startToInsert))
                {
                    return NotifyErrorAndRedirect("Deze werknemer heeft al een dienst op dat moment", "Create");
                }
                
                Context.Shifts.Add(
                    new Shift
                    {
                        Start = startToInsert,
                        End = endToInsert,
                        Department = (Department)department,
                        Employee = employee != 0 ? Context.Employees.Find(employee) : null,
                        IsFinal = isFinal.Value
                    }
                );
                Context.SaveChanges();
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het toevoegen van de dienst. Probeer het opnieuw", "Create", "Schedule");
            }
            
            return NotifySuccessAndRedirect("De dienst is toegevoegd", "Index", "Schedule");
        }

        public IActionResult Update()
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        public IActionResult MyShifts()
        {
            CheckPageAccess(Role.Employee);
            return View();
        }
    }
}
