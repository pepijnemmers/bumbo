using BumboApp.Helpers;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Numerics;
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
    public IActionResult Index(int? month, int? year)
    {
        ViewBag.Month = month;
        ViewBag.Year = year;
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
                continue;
            }

            if (workedHour.Status.Equals(HourStatus.Concept))
            {
                isConcept = true;
            }

            string userId = workedHour.Employee.UserId;
            string name = workedHour.Employee.FirstName;
            string workedTime = (workedHour.EndTime - workedHour.StartTime).Value.ToString();
            double bonusPercent = CalculateBonusPercent(workedHour);

            string dataString = $"{userId};{name};{workedTime};{bonusPercent}%";
            dataList.Add(dataString);
        }

        var excelStream = GenerateExcel(dataList);
        string fileName = $"gewerkteUren-{month}-{year}" + (isConcept ? "-CONCEPT" : "") + ".xlsx";
        return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    //Calculate the average bonus of a workedHour
    private double CalculateBonusPercent(WorkedHour workedHour)
    {
        var intervals = GetWorkedIntervals(workedHour);
        var bonusPeriods = GetBonusPeriods(workedHour.DateOnly.DayOfWeek);

        double totalWorkedTime = 0;
        double totalWeightedBonus = 0;
        foreach (var interval in intervals)
        {
            totalWorkedTime += (interval.End - interval.Start).TotalSeconds;
            foreach (var bonusPeriod in bonusPeriods)
            {
                totalWeightedBonus += CalculateWeightedTime(interval, bonusPeriod);
            }
        }
        double bonus = ((totalWeightedBonus / totalWorkedTime) - 1) * 100;
        return Math.Round(bonus, 1);
    }

    //Splits a workedHour into a list of intervals based on the breaks
    private List<(TimeOnly Start, TimeOnly End)> GetWorkedIntervals(WorkedHour workedHour)
    {
        List<Break> breaks = workedHour.Breaks ?? new();
        var intervals = new List<(TimeOnly Start, TimeOnly End)>();
        TimeOnly currentStart = workedHour.StartTime;
        foreach (var brk in breaks.OrderBy(b => b.StartTime))
        {
            if (brk != null && brk.EndTime.HasValue)
            {
                intervals.Add((currentStart, brk.StartTime));
                currentStart = brk.EndTime.Value;
            }
        }

        // Add the remaining time after the last break
        if (currentStart < workedHour.EndTime)
        {
            intervals.Add((currentStart, workedHour.EndTime.Value));
        }

        return intervals;
    }

    //calculate the amount of seconds*factor of a period of work and a bonus period
    private double CalculateWeightedTime((TimeOnly Start, TimeOnly End) period, BonusPeriod bonusPeriod)
    {
        if (period.Start >= bonusPeriod.EndTime || period.End <= bonusPeriod.StartTime)
        {
            return 0;
        }
        var overlapStart = Math.Max(period.Start.ToTimeSpan().TotalSeconds, bonusPeriod.StartTime.ToTimeSpan().TotalSeconds);
        var overlapEnd = Math.Min(period.End.ToTimeSpan().TotalSeconds, bonusPeriod.EndTime.ToTimeSpan().TotalSeconds);

        return (overlapEnd - overlapStart) * bonusPeriod.Multiplier;
    }

    //Should be retrieved from JSON, currently still here
    private static List<BonusPeriod> GetBonusPeriods(DayOfWeek dayNumber)
    {
        var result = new List<BonusPeriod>();
        switch (dayNumber)
        {
            case DayOfWeek.Sunday:
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(0, 0),
                    EndTime = new TimeOnly(23, 59),
                    Multiplier = 2d
                });
                break;
            case DayOfWeek.Saturday:
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(0, 0),
                    EndTime = new TimeOnly(6, 00),
                    Multiplier = 1.5d
                });
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(6, 0),
                    EndTime = new TimeOnly(18, 00),
                    Multiplier = 1d
                });
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(18, 0),
                    EndTime = new TimeOnly(23, 59),
                    Multiplier = 1.5d
                });
                break;
            default:
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(0, 0),
                    EndTime = new TimeOnly(6, 00),
                    Multiplier = 1.5d
                });
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(6, 0),
                    EndTime = new TimeOnly(20, 00),
                    Multiplier = 1d
                });
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(20, 0),
                    EndTime = new TimeOnly(21, 0),
                    //Multiplier = 4d / 3d,
                    Multiplier = 1.333d, //TODO
                });
                result.Add(new BonusPeriod
                {
                    StartTime = new TimeOnly(21, 0),
                    EndTime = new TimeOnly(23, 59),
                    Multiplier = 1.5d
                });
                break;
        }
        return result;
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