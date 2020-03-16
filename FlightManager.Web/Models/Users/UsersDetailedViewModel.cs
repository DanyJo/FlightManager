using FlightManager.Web.Models.Shared;
using FlightManager.Web.Models.View;
using System.Collections.Generic;

namespace FlightManager.Web.Models.Users
{
    public class UsersDetailedViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<UsersViewModel> Items { get; set; }
    }
}
