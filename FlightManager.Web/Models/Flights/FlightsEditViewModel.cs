using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Web.Models.Flights
{
    public class FlightsEditViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "The starting locationcannot cannot be longer than 20 characters")]
        public string StartLocation { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "The final locationcannot cannot be longer than 20 characters")]
        public string FinalLocation { get; set; }

        [Required]
        public DateTime TimeOfDepartment { get; set; }

        [Required]
        public DateTime TimeOfLanding { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "The plane type cannot be longer than 20 characters")]
        public string PlaneType { get; set; }

        [Required]
        public string PlaneNumber { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "The pilot name cannot be longer than 20 characters")]
        public string PilotName { get; set; }

        [Required]
        [Range(1,30, ErrorMessage = "There is not a single plane on Earth with more than 30 economy class seats")]
        public int EconomyClassCapacity { get; set; }

        [Required]
        [Range(1,30, ErrorMessage = "There is not a single plane on Earth with more than 30 buissness class seats")]
        public int BuissnessClassCapacity { get; set; }
    }
}
