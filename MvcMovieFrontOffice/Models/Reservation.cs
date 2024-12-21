using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovieFrontOffice.Models;

public class Reservation
{
    public Reservation(int id, int vehicleId, string? userId, DateTime startDate, DateTime endDate, string status, int totalPrice, DateTime? createdAt, DateTime? updatedAt)
    {
        Id = id;
        VehicleId = vehicleId;
        UserId = userId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        TotalPrice = totalPrice;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Reservation()
    {
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public int VehicleId { get; set; }

    [Required]
    public string? UserId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public string Status { get; set; }

    public int TotalPrice { get; set; }
    
    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; } 
}