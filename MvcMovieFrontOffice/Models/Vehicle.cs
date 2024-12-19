using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovieFrontOffice.Models;

public class Vehicle
{
    public Vehicle(int id, string model, string make, int year, bool? availability, int typeId, double price, DateTime? createdAt, DateTime? updatedAt)
    {
        Id = id;
        Model = model;
        Make = make;
        Year = year;
        Availability = availability;
        TypeId = typeId;
        Price = price;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    [Key] 
    public int Id { get; set; }

    [Required]
    public string Model { get; set; }

    [Required]
    public string Make { get; set; }

    [Required]
    public int Year { get; set; }

    public bool? Availability { get; set; }
    
    [Required]
    public double Price { get; set; }

    [Required]
    [Column("type_id")]
    public int TypeId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}