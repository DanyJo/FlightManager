using Microsoft.AspNetCore.Identity;

namespace FlightManager.Data.Models
{
    public class User : IdentityUser<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EGN { get; set; }

        public string Address { get; set; }

        public string Role { get; set; }
    }
}
