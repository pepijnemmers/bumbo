using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BumboApp.Controllers;

public class HoursExportController : MainController
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        CheckPageAccess(Role.Manager);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Download(int month, int year)
    {
        List<WorkedHour> workedHours = Context.WorkedHours.Where(wh => wh.DateOnly.Year == year && wh.DateOnly.Month == month).Include(wh => wh.Employee).ToList();
        List<string> dataList = new List<string>();

        foreach (var workedHour in workedHours)
        {
            dataList.Add(workedHour.Employee.UserId + "," + workedHour.Employee.FirstName + "," + (workedHour.EndTime - workedHour.StartTime) + "," + "TOESLAG");
            Console.WriteLine(workedHour.Employee.UserId + "," + workedHour.Employee.FirstName + "," + (workedHour.EndTime - workedHour.StartTime) + "," + "TOESLAG");
        }
        return RedirectToAction(nameof(Index));
    }

    private double CalculateBonus()
    {

        return 0;
    }

    private MemoryStream GenerateExcel(List<string> inputData)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Gewerkte uren");
            //HEADERS (onnodig ofniet?)
            //worksheet.Cells[1, 1].Value = "BID";
            //worksheet.Cells[1, 2].Value = "Naam";
            //worksheet.Cells[1, 3].Value = "Gewerkte uren";
            //worksheet.Cells[1, 4].Value = "Toeslag";


            int row = 1;
            foreach (var line in inputData)
            {
                string[] columns = line.Split(',');
                for (int col = 0; col < columns.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value = columns[col];
                }
                row++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}