using FlightManager.Data;
using FlightManager.Data.Models;
using FlightManager.Web.Models.Flights;
using FlightManager.Web.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Web.Controllers
{
    [Authorize]
    public class FlightsController : Controller
    {
        private readonly FlightManagerDbContext context;
        private const int PAGE_SIZE = 1;

        public FlightsController(FlightManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(FlightsIndexViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<FlightsViewModel> items = await context.Flights.Skip((model.Pager.CurrentPage - 1) * PAGE_SIZE).Take(PAGE_SIZE).Select(f => new FlightsViewModel()
            {
                Id = f.Id,
                StartLocation = f.StartLocation,
                FinalLocation = f.FinalLocation,
                TimeOfDepartment = f.TimeOfDepartment,
                TimeOfLanding = f.TimeOfLanding,
                TravelingTime = Math.Round((f.TimeOfLanding - f.TimeOfDepartment).TotalHours, 2),
                PlaneType = f.PlaneType,
                PlaneNumber = f.PlaneNumber,
                PilotName = f.PilotName,
                EconomyClassCapacity = f.EconomyClassCapacity,
                BuissnessClassCapacity = f.BuissnessClassCapacity
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await context.Flights.CountAsync() / (double)PAGE_SIZE);

            return View("FlightsView", model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            FlightsCreateViewModel model = new FlightsCreateViewModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(FlightsCreateViewModel model)
        {
            if ((model.TimeOfLanding - model.TimeOfDepartment).TotalMinutes < 30)
            {
                ModelState.AddModelError(nameof(model.TimeOfLanding), "The shortest flight is 30 minutes");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            Flight flight = new Flight()
            {
                Id = Guid.NewGuid().ToString(),
                StartLocation = model.StartLocation,
                FinalLocation = model.FinalLocation,
                TimeOfDepartment = model.TimeOfDepartment,
                TimeOfLanding = model.TimeOfLanding,
                PlaneType = model.PlaneType,
                PlaneNumber = model.PlaneNumber,
                PilotName = model.PilotName,
                EconomyClassCapacity = model.EconomyClassCapacity,
                BuissnessClassCapacity = model.BuissnessClassCapacity
            };

            await this.context.AddAsync(flight);
            await this.context.SaveChangesAsync();

            return Redirect("~/Home");
        }

        [HttpGet]
        [ActionName("Read")]
        public async Task<IActionResult> Read(string id)
        {
            Flight flight = await context.Flights.SingleOrDefaultAsync(f => f.Id == id);


            FlightsViewModel flightsViewModel = new FlightsViewModel()
            {
                Id = flight.Id,
                StartLocation = flight.StartLocation,
                FinalLocation = flight.FinalLocation,
                TimeOfDepartment = flight.TimeOfDepartment,
                TimeOfLanding = flight.TimeOfLanding,
                PlaneType = flight.PlaneType,
                PlaneNumber = flight.PlaneNumber,
                PilotName = flight.PilotName,
                EconomyClassCapacity = flight.EconomyClassCapacity,
                BuissnessClassCapacity = flight.BuissnessClassCapacity
            };

            return View("Details", flightsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id)
        {
            Flight flight = await this.context.Flights.SingleOrDefaultAsync(x => x.Id == id);

            FlightsEditViewModel flightsEditViewModel = new FlightsEditViewModel()
            {
                Id = flight.Id,
                StartLocation = flight.StartLocation,
                FinalLocation = flight.FinalLocation,
                TimeOfDepartment = flight.TimeOfDepartment,
                TimeOfLanding = flight.TimeOfLanding,
                PlaneType = flight.PlaneType,
                PlaneNumber = flight.PlaneNumber,
                PilotName = flight.PilotName,
                EconomyClassCapacity = flight.EconomyClassCapacity,
                BuissnessClassCapacity = flight.BuissnessClassCapacity

            };

            return View(flightsEditViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id, FlightsEditViewModel flightsEditViewModel)
        {
            if ((flightsEditViewModel.TimeOfLanding - flightsEditViewModel.TimeOfDepartment).TotalMinutes < 30)
            {
                ModelState.AddModelError(nameof(flightsEditViewModel.TimeOfLanding), "The  shortest flight is 30 minutes");
            }

            if (!ModelState.IsValid)
            {
                return View(flightsEditViewModel);
            }

            Flight flight = await this.context.Flights.SingleOrDefaultAsync(x => x.Id == id);

            flight.StartLocation = flightsEditViewModel.StartLocation;
            flight.FinalLocation = flightsEditViewModel.FinalLocation;
            flight.TimeOfDepartment = flightsEditViewModel.TimeOfDepartment;
            flight.TimeOfLanding = flightsEditViewModel.TimeOfLanding;
            flight.PlaneType = flightsEditViewModel.PlaneType;
            flight.PlaneNumber = flightsEditViewModel.PlaneNumber;
            flight.PilotName = flightsEditViewModel.PilotName;
            flight.EconomyClassCapacity = flightsEditViewModel.EconomyClassCapacity;
            flight.BuissnessClassCapacity = flightsEditViewModel.BuissnessClassCapacity;

            this.context.Update(flight);
            await this.context.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(string id)
        {
            Flight flight = await this.context.Flights.FindAsync(id);
            this.context.Remove(flight);

            await this.context.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> PassengerDetails(string id)
        {
            List<Reservation> passengers = context.Reservations.Where(r => r.FlightId == id).ToList();

            FlightsPassengerViewModel model = new FlightsPassengerViewModel()
            {
                Passengers = passengers
            };



            return RedirectToAction("Index", "Reservation", model, id);
        }

    }
}
