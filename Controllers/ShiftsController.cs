using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        
        [HttpGet]
        public IActionResult Update(int id)
        {
            CheckPageAccess(Role.Manager);
            
            var shift = Context.Shifts.Find(id);
            if (shift == null)
            {
                return NotifyErrorAndRedirect("De dienst kon niet worden gevonden", "Index", "Schedule");
            }
            if (shift.Start < DateTime.Today)
            {
                NotifyService.Error("De dienst kan niet worden aangepast, omdat deze in het verleden is.");
                return RedirectToAction("Index", "Schedule", new { date = shift.Start });
            }
            
            var viewModel = new CreateUpdateShiftViewModel
            {
                Shift = shift,
                Departments = Enum.GetValues<Department>().ToList(),
                Employees = Context.Employees
                    .ToList()
            };
            return View(viewModel);
        }

        public IActionResult MyShifts()
        {
            CheckPageAccess(Role.Employee);
            return View();
        }

        [HttpPost]
        public IActionResult Create(DateOnly? date, TimeOnly? start, TimeOnly? end, Department? department, int? employee, bool? isFinal)
        {
            // validation (TODO: check if is allowed cao)
            bool valid = true;
            if (date == null || start == null || end == null || department == null || employee == null || isFinal == null)
            {
                NotifyService.Error("Niet alle velden zijn ingevuld");
                valid = false;
            }
            if (date < DateOnly.FromDateTime(DateTime.Today) && valid)
            {
                NotifyService.Error("De datum moet in de toekomst zijn");
                valid = false;
            }
            if (start >= end && valid)
            {
                NotifyService.Error("De eindtijd moet na de starttijd zijn");
                valid = false;
            }
            if (Enum.TryParse<Department>(department.ToString(), out var dep) == false && valid)
            {
                NotifyService.Error("De afdeling is niet geldig");
                valid = false;
            }
            
            if (valid == false)
            {
                return RedirectToAction("Create", new
                {
                    date = date?.ToString(),
                    start = start?.ToString(),
                    end = end?.ToString(),
                    department = department?.ToString(),
                    employee = employee?.ToString(),
                    isFinal = isFinal?.ToString()
                });
            }
            
            // add to database
            try
            {
                var startToInsert = new DateTime(date!.Value, start!.Value);
                var endToInsert = new DateTime(date.Value, end!.Value);
                    
                // check if employee already has a shift at that time
                if (employee != 0 && 
                    Context.Shifts
                        .Any(s => s.Employee != null
                                  && s.Employee.EmployeeNumber == employee
                                  && s.Start < endToInsert
                                  && s.End > startToInsert))
                {
                    NotifyService.Error("Deze werknemer heeft al een dienst op dat moment");
                    return RedirectToAction("Create", new
                    {
                        date = date?.ToString(),
                        start = start?.ToString(),
                        end = end?.ToString(),
                        department = department?.ToString(),
                        employee = employee?.ToString(),
                        isFinal = isFinal?.ToString()
                    });
                }
                
                Context.Shifts.Add(
                    new Shift
                    {
                        Start = startToInsert,
                        End = endToInsert,
                        Department = (Department)department!,
                        Employee = employee != 0 ? Context.Employees.Find(employee) : null,
                        IsFinal = isFinal!.Value
                    }
                );
                Context.SaveChanges();
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het toevoegen van de dienst. Probeer het opnieuw", "Create", "Schedule");
            }
            NotifyService.Success("De dienst is toegevoegd");
            return RedirectToAction("Index", "Schedule", new { date = date.Value.ToString("dd-MM-yyyy") });
        }
        
        [HttpPost]
        public IActionResult Update(int id, DateOnly date, TimeOnly start, TimeOnly end, Department department, string? employeeNumber, bool isFinal)
        {
            var shiftStart = new DateTime(date, start);
            var shiftEnd = new DateTime(date, end);
            
            // validation (TODO: check if is allowed cao)
            if (ModelState.IsValid == false)
            {
                NotifyService.Error("Niet alle velden zijn ingevuld");
                return RedirectToAction("Update", new { id = id });
            }
            if (shiftStart.Date < DateTime.Today)
            {
                NotifyService.Error("De datum moet in de toekomst zijn");
                return RedirectToAction("Update", new { id = id });
            }
            if (shiftStart >= shiftEnd)
            {
                NotifyService.Error("De eindtijd moet na de starttijd zijn");
                return RedirectToAction("Update", new { id = id });
            }
            var employee = employeeNumber != null ? Context.Employees.Find(int.Parse(employeeNumber)) : null;
            if (employeeNumber != null && employee == null)
            {
                NotifyService.Error("De werknemer kon niet worden gevonden");
                return RedirectToAction("Update", new { id = id });
            }
            
            // check if employee already has a shift at that time
            if (employee != null && 
                Context.Shifts
                    .Any(s => s.Employee != null
                              && s.Id != id
                              && s.Employee == employee
                              && s.Start < shiftEnd
                              && s.End > shiftStart))
            {
                NotifyService.Error("Deze werknemer heeft al een dienst op dat moment");
                return RedirectToAction("Update", new { id = id });
            }
            
            // update in database
            try
            {
                var shift = Context.Shifts.Find(id);
                if (shift == null)
                {
                    NotifyService.Error("De dienst kon niet worden gevonden");
                    return RedirectToAction("Update", new { id = id });
                }
                
                shift.Start = shiftStart;
                shift.End = shiftEnd;
                shift.Department = department;
                shift.Employee = employee;
                shift.IsFinal = isFinal;
                
                Context.Shifts.Update(shift);
                Context.SaveChanges();
            }
            catch
            {
                NotifyService.Error("Er is iets fout gegaan bij het wijzigen van de dienst. Probeer het opnieuw");
                return RedirectToAction("Update", new { id = id });
            }
            NotifyService.Success("De dienst is gewijzigd");
            return RedirectToAction("Index", "Schedule", new { startDate = date.ToString("dd/MM/yyyy") });
        }
        
        [HttpPost]
        public IActionResult Delete(Shift shift)
        {
            // TODO delete shift and redirect to schedule page to corresponding date
            NotifyService.Success("De dienst is verwijderd");
            return RedirectToAction("Index", "Schedule", new { startDate = "2024-12-02" });
        }
    }
}
