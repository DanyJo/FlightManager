using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FlightManager.Data;
using FlightManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightManager.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly FlightManagerDbContext flightManagerDbContext;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            FlightManagerDbContext flightManagerDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.flightManagerDbContext = flightManagerDbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public class InputModel
        {
            [Required]
            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(17, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [RegularExpression(@"^[0-9]{10}", ErrorMessage = "EGN should be only digits and exatcly 10 characters long")]
            [Display(Name = "EGN")]
            public string EGN { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required]
            [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "The must be at either 10 or 13 characters long.")]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new User { Id = Guid.NewGuid().ToString(), UserName = Input.Username, FirstName = Input.FirstName, LastName = Input.LastName, Email = Input.Email, EGN = Input.EGN, Address = Input.Address, PhoneNumber = Input.PhoneNumber };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {

                    if (this.flightManagerDbContext.Users.Count() == 1)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                       
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
