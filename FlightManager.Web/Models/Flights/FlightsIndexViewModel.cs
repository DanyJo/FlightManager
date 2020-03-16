using FlightManager.Web.Models.Shared;
using System.Collections.Generic;

namespace FlightManager.Web.Models.Flights
{
    public class FlightsIndexViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<FlightsViewModel> Items { get; set; }
    }
}
