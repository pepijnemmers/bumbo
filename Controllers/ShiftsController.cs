using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

                var employeeToInsert = employee != 0 ? Context.Employees.Find(employee) : null;
                Context.Shifts.Add(
                    new Shift
                    {
                        Start = startToInsert,
                        End = endToInsert,
                        Department = (Department)department!,
                        Employee = employeeToInsert,
                        IsFinal = isFinal!.Value
                    }
                );

                if ((bool)isFinal && employeeToInsert != null)
                {
                    Context.Notifications.Add(new Notification
                    {
                        Employee = employeeToInsert,
                        Title = $"Dienst {department} toegevoegd - {startToInsert.ToString("dd-MM-yyyy")}",
                        Description = $"Er is een dienst voor jou toegevoegd op {startToInsert.ToString("dd-MM-yyyy")}",
                        SentAt = DateTime.Now,
                        HasBeenRead = false,
                        ActionUrl = $"/Schedule?startDate={startToInsert.ToString("dd/MM/yyyy")}"
                    });
                }
                Context.SaveChanges();
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het toevoegen van de dienst. Probeer het opnieuw", "Create", "Schedule");
            }
            NotifyService.Success("De dienst is toegevoegd");
            return RedirectToAction("Index", "Schedule", new { startDate = date.Value.ToString("dd/MM/yyyy") });
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
            if (employeeNumber == null || employee == null)
            {
                NotifyService.Error("De werknemer kon niet worden gevonden");
                return RedirectToAction("Update", new { id = id });
            }
            
            // check if employee already has a shift at that time
            if (Context.Shifts
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

                bool conceptMadeFinal = !shift.IsFinal && isFinal;
                
                shift.Start = shiftStart;
                shift.End = shiftEnd;
                shift.Department = department;
                shift.Employee = employee;
                shift.IsFinal = isFinal;
                Context.Shifts.Update(shift);

                if (conceptMadeFinal)
                {
                    Context.Notifications.Add(new Notification
                    {
                        Employee = employee,
                        Title =
                            $"Dienst {shift.Department.ToFriendlyString()} toegevoegd - {shift.Start.ToString("dd/MM/yyyy")}",
                        Description =
                            $"Er is een dienst voor jou toegevoegd op {shift.Start.ToString("dd/MM/yyyy")}.",
                        SentAt = DateTime.Now,
                        HasBeenRead = false,
                        ActionUrl = $"/Schedule?startDate={shift.Start.ToString("dd/MM/yyyy")}"
                    });
                }
                else
                {
                    Context.Notifications.Add(new Notification
                    {
                        Employee = employee,
                        Title =
                            $"Dienst {shift.Department.ToFriendlyString()} gewijzigd - {shift.Start.ToString("dd/MM/yyyy")}",
                        Description =
                            $"Je dienst op {shift.Start.ToString("dd/MM/yyyy")} van {shift.Start.ToString("HH:mm")} tot {shift.End.ToString("HH:mm")} is gewijzigd.",
                        SentAt = DateTime.Now,
                        HasBeenRead = false,
                        ActionUrl = $"/Schedule?startDate={shift.Start.ToString("dd/MM/yyyy")}"
                    });
                }
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                
                NotifyService.Error("Er is iets fout gegaan bij het wijzigen van de dienst. Probeer het opnieuw");
                return RedirectToAction("Update", new { id = id });
            }
            NotifyService.Success("De dienst is gewijzigd");
            return RedirectToAction("Index", "Schedule", new { startDate = date.ToString("dd/MM/yyyy") });
        }
        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var shiftToDelete = Context.Shifts.Include(s => s.Employee).FirstOrDefault(std => std.Id == id);
            
            // validation
            if (shiftToDelete == null)
            {
                return NotifyErrorAndRedirect("De dienst kon niet worden gevonden", "Index", "Schedule");
            }
            if (shiftToDelete.Start < DateTime.Today)
            {
                return NotifyErrorAndRedirect("De dienst kan niet worden verwijderd, omdat deze in het verleden is.", "Index", "Schedule");
            }
            
            // delete from database
            try
            {
                Context.Shifts.Remove(shiftToDelete);
                
                if (shiftToDelete.Employee != null)
                {
                    Context.Notifications.Add(new Notification
                    {
                        Employee = shiftToDelete.Employee,
                        Title =
                            $"Dienst {shiftToDelete.Department.ToFriendlyString()} verwijderd - {shiftToDelete.Start.ToString("dd/MM/yyyy")}",
                        Description =
                            $"Je dienst op {shiftToDelete.Start.ToString("dd/MM/yyyy")} van {shiftToDelete.Start.ToString("HH:mm")} tot {shiftToDelete.End.ToString("HH:mm")} is verwijderd.",
                        SentAt = DateTime.Now,
                        HasBeenRead = false,
                        ActionUrl = $"/Schedule?startDate={shiftToDelete.Start.ToString("dd/MM/yyyy")}"
                    });   
                }
                
                Context.SaveChanges();
            }
            catch
            {
                NotifyService.Error("Er is iets fout gegaan bij het verwijderen van de dienst. Probeer het opnieuw");
                return RedirectToAction("Update", new { id = id });
            }
            
            NotifyService.Success("De dienst is verwijderd");
            return RedirectToAction("Index", "Schedule", new { startDate = shiftToDelete.Start.ToString("dd/MM/yyyyy") });
        }
    }
}
