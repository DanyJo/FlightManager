using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Web.Models.View
{
    public class UsersViewModel
    {
        [HiddenInput]

        public string Id { get; set; }


        [Required]
        [MaxLength(20, ErrorMessage = "The username cannot be longer than 20 characters")]

        public string Username { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "There aren't names longer than 15 characters")]

        public string FirstName { get; set; }

        [Required]
        [MaxLength(17, ErrorMessage = "There aren't last names longer than 15 characters")]

        public string LastName { get; set; }

        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}", ErrorMessage = "EGN should be only digits and exatcly 10 characters long")]

        public string EGN { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The address cannot be longer than 100 characters.")]

        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "The must be at either 10 or 13 characters long.")]

        public string PhoneNumber { get; set; }
    }
}
