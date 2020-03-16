using System;

namespace FlightManager.Data.Models
{
    public class Flight
    {
        public string Id { get; set; }

        public string StartLocation { get; set; }

        public string FinalLocation { get; set; }

        public DateTime TimeOfDepartment { get; set; }

        public DateTime TimeOfLanding { get; set; }

        public string PlaneType { get; set; }

        public string PlaneNumber { get; set; }

        public string PilotName { get; set; }

        public int EconomyClassCapacity { get; set; }

        public int BuissnessClassCapacity { get; set; }
    }
}
