using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovieFrontOffice.Models;

public class Reservation
{
    public Reservation(int id, int vehicleId, string userId, DateTime startDate, DateTime endDate, string status, decimal totalPrice, DateTime? createdAt, DateTime? updatedAt)
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

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("vehicle_id")]
    public int VehicleId { get; set; }

    [Required]
    [Column("user_id")]
    public string UserId { get; set; }

    [Required]
    [Column("start_date", TypeName = "date")]
    public DateTime StartDate { get; set; }
    
    [Required]
    [Column("end_date", TypeName = "date")]
    public DateTime EndDate { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("total_price")]
    public decimal TotalPrice { get; set; }
    
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } 
}