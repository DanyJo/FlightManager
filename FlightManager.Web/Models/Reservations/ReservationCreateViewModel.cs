using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Web.Models.Reservations
{
    public class ReservationCreateViewModel
    {
        [Required]
        [MaxLength(15, ErrorMessage = "There aren't names longer than 15 characters")]

        public string FirstName { get; set; }

        [Required]
        [MaxLength(17, ErrorMessage = "There aren't last names longer than 15 characters")]

        public string MiddleName { get; set; }

        [Required]
        [MaxLength(17, ErrorMessage = "There aren't last names longer than 15 characters")]

        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}", ErrorMessage = "EGN should be only digits and exatcly 10 characters long")]

        public string EGN { get; set; }

        [Required]
        [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "The must be at either 10 or 13 characters long.")]

        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Your nationality cannot be longer than 25 characters")]

        public string Nationality { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Your email cannot be longer than 15 characters")]

        public string Email { get; set; }

        [HiddenInput]
        public string FlightId { get; set; }

        [Required]
        public bool isBuissnessClass { get; set; }

        public bool hasBuissnessSeats { get; set; }
    }


}
