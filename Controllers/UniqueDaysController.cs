﻿using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class UniqueDaysController : MainController
    {
        private int maxFactor = 100;
        public IActionResult Edit(int id)
        {
            UniqueDay? uniqueDay = Context.UniqueDays.Find(id);
            return uniqueDay == null ? NotifyErrorAndRedirect("De Speciale dag die je probeert te bewerken bestaat niet.", "Index", "OpeningHours") : View(uniqueDay);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(UniqueDay uniqueDay)
        {
            // validation
            if (uniqueDay.StartDate > uniqueDay.EndDate)
                return NotifyErrorAndRedirect("EindDatum mag niet voor de StartDatum liggen.", "Index", "OpeningHours");

            if (Read().Find(u => u.StartDate == uniqueDay.StartDate && u.Name == uniqueDay.Name && uniqueDay.Id != u.Id) != null)
                return NotifyErrorAndRedirect("Deze speciale dag bestaat al.", "Index", "OpeningHours");

            if (uniqueDay.StartDate <= DateOnly.FromDateTime(DateTime.Now))
                return NotifyErrorAndRedirect("De data moet in de toekomst liggen.", "Index", "OpeningHours");

            if (uniqueDay.Factor > maxFactor)
            {
                return NotifyErrorAndRedirect("De factor mag niet meer zijn dan " + maxFactor + ".", "Index", "OpeningHours");
            }

            if (!(ModelState.IsValid))
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Index", "OpeningHours");

            // update to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                var existingDay = Context.UniqueDays.Find(uniqueDay.Id);
                if (existingDay != null)
                {
                    Context.Entry(existingDay).CurrentValues.SetValues(uniqueDay);
                }
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De Speciale dag is bijgewerkt.");
            }
            catch (Exception e)
            {
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het bewerken van de Speciale dag.");
            }

            return RedirectToAction("index","OpeningHours");
        }

        [HttpGet]
        public List<UniqueDay> Read()
        {
            List<UniqueDay> uniqueDays = Context.UniqueDays
                .OrderBy(e => e.StartDate)
                .ToList();

            return uniqueDays;
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var uniqueDay = Context.UniqueDays.Find(id);
            if (uniqueDay == null)
            {
                return NotifyErrorAndRedirect("Speciale dag niet gevonden.", "Index", "OpeningHours");
            }
            if (uniqueDay.StartDate <= DateOnly.FromDateTime(DateTime.Now))
            {
                return NotifyErrorAndRedirect("Speciale dag al geweest.", "Index", "OpeningHours");
            }

            try
            {
                Context.UniqueDays.Remove(uniqueDay);
                Context.SaveChanges();
                NotifyService.Success("De Speciale Dag is Verwijderd");
            }
            catch (Exception ex)
            {
                return NotifyErrorAndRedirect("Fout bij verwijderen Speciale dag.", "Index", "OpeningHours");
            }

            return RedirectToAction("Index","OpeningHours");
        }

        [HttpPost]
        public IActionResult Create(UniqueDay uniqueDay)
        {
            // validation
            if (Read().Find(u => u.StartDate == uniqueDay.StartDate && u.Name.ToLower() == uniqueDay.Name.ToLower()) != null)
                return NotifyErrorAndRedirect("De Speciale dag die je probeert toe te voegen bestaat al.", "Index", "OpeningHours");

            if (uniqueDay.StartDate > uniqueDay.EndDate)
                return NotifyErrorAndRedirect("De startdatum moet voor of op de einddatum vallen.", "Add");

            if (uniqueDay.StartDate <= DateOnly.FromDateTime(DateTime.Now))
                return NotifyErrorAndRedirect("De Data van de Speciale dag moet in de toekomst liggen.", "Add");

            if (uniqueDay.Factor <= 0)
                return NotifyErrorAndRedirect("de Factor moet boven 0 liggen.", "Add");

            if (uniqueDay.Factor > maxFactor)
                return NotifyErrorAndRedirect("de Factor mag niet groter zijn dan " + maxFactor + ".", "Add");

            if (!ModelState.IsValid)
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Add");

            // add to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                Context.UniqueDays.Add(uniqueDay);
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De Speciale dag is toegevoegd!");
            }
            catch
            {
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het toevoegen van de Speciale dag.");
            }

            return RedirectToAction("Index", "OpeningHours");
        }
    }
}

