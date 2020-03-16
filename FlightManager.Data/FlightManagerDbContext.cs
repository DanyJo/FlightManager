using FlightManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Data
{
    public class FlightManagerDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public FlightManagerDbContext(DbContextOptions<FlightManagerDbContext> options)
         : base(options)
        { }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FlightManagerDb;Trusted_Connection=True;");

            
        }
    }
}
