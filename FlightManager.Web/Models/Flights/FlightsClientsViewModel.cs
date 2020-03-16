using System;

namespace FlightManager.Web.Models.Flights
{
    public class FlightsClientsViewModel
    {
        public string Id { get; set; }

        public string StartLocation { get; set; }

        public string FinalLocation { get; set; }

        public DateTime TimeOfDepartment { get; set; }

        public DateTime TimeOfLanding { get; set; }

        public double TravelingTime { get; set; }

        public int EconomyClassCapacity { get; set; }

        public int BuissnessClassCapacity { get; set; }
    }
}
