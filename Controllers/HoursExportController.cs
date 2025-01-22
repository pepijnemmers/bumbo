using BumboApp.Helpers;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace BumboApp.Controllers;

public class HoursExportController : MainController
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        CheckPageAccess(Role.Manager);
    }

    [HttpGet]
    public IActionResult Index(int? month, int? year)
    {
        ViewBag.Month = month;
        ViewBag.Year = year;

        var workedHours = Context.WorkedHours;
        Dictionary<int, List<int>> yearToMonths = workedHours
            .GroupBy(workedHour => workedHour.DateOnly.Year)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(WorkedHour => WorkedHour.DateOnly.Month)
                    .Distinct()
                    .OrderBy(month => month)
                    .ToList()
            );

        ViewData["PossibleCombinations"] = yearToMonths;
        return View();
    }

    [HttpPost]
    public IActionResult CanDownload(int month, int year)
    {
        List<WorkedHour> workedHours = Context.WorkedHours.Where(wh => wh.DateOnly.Year == year && wh.DateOnly.Month == month).Include(wh => wh.Employee).ToList();

        if (workedHours == null || !workedHours.Any())
        {
            NotifyService.Error("Er is geen data van deze periode");
            return RedirectToAction(nameof(Index), new { month, year });
        }

        return Download(month, year);
    }

    [HttpPost]
    public IActionResult Download(int month, int year)
    {
        List<WorkedHour> workedHours = Context.WorkedHours.Where(wh => wh.DateOnly.Year == year && wh.DateOnly.Month == month)
            .Include(wh => wh.Employee)
            .Include(wh => wh.Breaks).ToList();

        List<string> dataList = new List<string>();
        bool isConcept = false;
        foreach (var workedHour in workedHours)
        {
            if (workedHour.EndTime == null || workedHour.Status.Equals(HourStatus.Concept))
            {
                isConcept = true;
                continue;
            }

            string userId = workedHour.Employee.UserId;
            string name = workedHour.Employee.FirstName;
            string workedTime = (workedHour.EndTime - workedHour.StartTime).Value.ToString();
            double bonusPercent = ExportHoursHelper.CalculateBonusPercent(workedHour);

            string dataString = $"{userId};{name};{workedTime};{bonusPercent}%";
            dataList.Add(dataString);
        }

        var excelStream = GenerateExcel(dataList);
        string fileName = $"gewerkteUren-{month}-{year}" + (isConcept ? "-CONCEPT" : "") + ".xlsx";
        return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    private static MemoryStream GenerateExcel(List<string> inputData)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Gewerkte uren");
            worksheet.Cells[1, 1].Value = "BID";
            worksheet.Cells[1, 2].Value = "Naam";
            worksheet.Cells[1, 3].Value = "Gewerkte uren";
            worksheet.Cells[1, 4].Value = "Toeslag";

            int row = 2;
            foreach (var line in inputData)
            {
                string[] columns = line.Split(';');
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