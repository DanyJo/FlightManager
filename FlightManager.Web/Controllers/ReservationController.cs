using EmailService;
using FlightManager.Data;
using FlightManager.Data.Models;
using FlightManager.EmailService;
using FlightManager.Web.Models.Flights;
using FlightManager.Web.Models.Reservations;
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
    public class ReservationController : Controller
    {
        private readonly FlightManagerDbContext context;
        private readonly EmailConfiguration emailConfiguration;
        private const int PAGE_SIZE = 1;
        public ReservationController(FlightManagerDbContext context, EmailConfiguration emailConfiguration)
        {
            this.context = context;
            this.emailConfiguration = emailConfiguration;
        }


        public async Task<IActionResult> Index(string id, FlightsPassengerViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            int passengersCount = await this.context.Reservations.Where(x => x.FlightId == id).CountAsync();

            List<Reservation> passengers = await context.Reservations.Where(x => x.FlightId == id).Skip((model.Pager.CurrentPage - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToListAsync();

            model.Passengers = passengers;
            model.Pager.PagesCount = (int)Math.Ceiling(await context.Reservations.CountAsync() / (double)PAGE_SIZE);

            if (model.Pager.PagesCount >passengersCount)
            {
                model.Pager.PagesCount = passengersCount;
            }

            return View("ReservationView", model);
        }

        [HttpGet]
        public IActionResult Create(string id)
        {
            ReservationCreateViewModel model = new ReservationCreateViewModel()
            {
                FlightId = id,
                hasBuissnessSeats = this.context.Flights.FindAsync(id).Result.BuissnessClassCapacity == 0 ? false : true
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationCreateViewModel model)
        {
            if (this.context.Flights.FindAsync(model.FlightId).Result.EconomyClassCapacity == 0 && !model.isBuissnessClass)
            {
                ModelState.AddModelError(nameof(model.isBuissnessClass), "There aren't any economy seats left");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            string ticketType = model.isBuissnessClass ? "Buissness class" : "Economy class";

            Message message = new Message(new string[] { model.Email }, "Flight reservation",
                $"Hello {model.FirstName} {model.LastName},\n" +
                $"You have successfully booked your {ticketType} tickets to {this.context.Flights.FindAsync(model.FlightId).Result.FinalLocation}");

            EmailSender emailSender = new EmailSender(this.emailConfiguration);
            await emailSender.SendEmailAsync(message);


            Reservation reservation = new Reservation()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                EGN = model.EGN,
                PhoneNumber = model.PhoneNumber,
                Nationality = model.Nationality,
                FlightId = model.FlightId,
                TicketType = ticketType
            };

            await this.context.AddAsync(reservation);

            if (reservation.TicketType == "Economy class")
            {
                this.context.Flights.FindAsync(reservation.FlightId).Result.EconomyClassCapacity--;
            }
            else
            {
                this.context.Flights.FindAsync(reservation.FlightId).Result.BuissnessClassCapacity--;
            }

            await this.context.SaveChangesAsync();

            return Redirect("~/Home");
        }

        [HttpPost]
        [ActionName("Delete")]

        public async Task<IActionResult> Delete(string id)
        {
            Reservation reservation = await this.context.Reservations.FindAsync(id);
            this.context.Remove(reservation);

            if (reservation.TicketType == "Economy class")
            {
                this.context.Flights.FindAsync(reservation.FlightId).Result.EconomyClassCapacity++;
            }
            else
            {
                this.context.Flights.FindAsync(reservation.FlightId).Result.BuissnessClassCapacity++;
            }

            await this.context.SaveChangesAsync();

            return Redirect("/");
        }
    }
}
