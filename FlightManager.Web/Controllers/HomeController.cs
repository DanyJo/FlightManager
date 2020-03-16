using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightManager.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using FlightManager.Web.Models.Flights;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlightManagerDbContext context;
        // private readonly RoleManager<IdentityRole> roleManager;
        public HomeController(FlightManagerDbContext flightManagerDbContext)//, RoleManager<IdentityRole> roleManager)
        {
            this.context = flightManagerDbContext;
            // this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            /*  IdentityRole userRole = new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "User" };
              IdentityRole adminRole = new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "Admin" };

              await this.roleManager.CreateAsync(userRole);
              await this.roleManager.CreateAsync(adminRole);*/

            if (this.User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Flights");
            }
            else
            {
                List<FlightsClientsViewModel> InitilaPageModels = await this.context.Flights
               .Select(flight => new FlightsClientsViewModel
               {
                   Id = flight.Id,
                   StartLocation = flight.StartLocation,
                   FinalLocation = flight.FinalLocation,
                   TimeOfDepartment = flight.TimeOfDepartment,
                   TimeOfLanding = flight.TimeOfLanding,
                   TravelingTime = Math.Round((flight.TimeOfLanding - flight.TimeOfDepartment).TotalHours, 2),
                   EconomyClassCapacity = flight.EconomyClassCapacity,
                   BuissnessClassCapacity = flight.BuissnessClassCapacity
               }).ToListAsync();

                return View("Initial", InitilaPageModels);
            }
        }
    }
}
