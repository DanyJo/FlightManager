using FlightManager.Data.Models;
using FlightManager.Web.Models.Shared;
using System.Collections.Generic;

namespace FlightManager.Web.Models.Flights
{
    public class FlightsPassengerViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<Reservation> Passengers { get; set; }
    }
}
