using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightManager.Data;
using FlightManager.Data.Models;
using FlightManager.Web.Models.Shared;
using FlightManager.Web.Models.Users;
using FlightManager.Web.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly FlightManagerDbContext context;
        private const int PAGE_SIZE = 1;

        public UserController(FlightManagerDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Read(UsersDetailedViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<UsersViewModel> items = await context.Users.Skip((model.Pager.CurrentPage - 1) * PAGE_SIZE).Take(PAGE_SIZE).Select(user => new UsersViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EGN = user.EGN,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await context.Users.CountAsync() / (double)PAGE_SIZE);

            return View("UserView", model);


            /* List<UsersViewModel> usersViewModels = await this.context.Users
                    .Select(user => new UsersViewModel
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        EGN = user.EGN,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber

                    }).ToListAsync();

             return this.View("Details", usersViewModels);*/
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id)
        {
            User user = await this.context.Users.SingleOrDefaultAsync(u => u.Id == id);

            UsersViewModel usersViewModel = new UsersViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EGN = user.EGN,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return View("Edit",usersViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id, UsersViewModel usersViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            User user = await this.context.Users.SingleOrDefaultAsync(u => u.Id == id);

            user.Id = usersViewModel.Id;
            user.UserName = usersViewModel.Username;
            user.FirstName = usersViewModel.FirstName;
            user.LastName = usersViewModel.LastName;
            user.Email = usersViewModel.Email;
            user.EGN = usersViewModel.EGN;
            user.Address = usersViewModel.Address;
            user.PhoneNumber = usersViewModel.PhoneNumber;

            this.context.Update(user);
            await this.context.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(string id)
        {
            User user = await this.context.Users.FindAsync(id);
            this.context.Remove(user);

            await this.context.SaveChangesAsync();

            return Redirect("/");
        }
    }
}