using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.Models;

public class ReservationViewModel()
{
    [Required]
    public Reservation Reservation { get; set; }
    
    public VehicleView VehicleView { get; set; }
    
}