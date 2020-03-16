namespace FlightManager.Data.Models
{
    public class Reservation
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string EGN { get; set; }

        public string PhoneNumber{ get; set; }

        public string Nationality { get; set; }

        public string TicketType { get; set; }

        public string FlightId { get; set; }

        public virtual Flight Flight { get; set; }
    }
}
