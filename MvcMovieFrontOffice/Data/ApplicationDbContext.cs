using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Data;

public class ApplicationDbContext: IdentityDbContext<Users>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public virtual DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleView> VehicleView { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<VehicleImage> VehicleImages { get; set; }
    public DbSet<ReservationView> ReservationView { get; set; }

}