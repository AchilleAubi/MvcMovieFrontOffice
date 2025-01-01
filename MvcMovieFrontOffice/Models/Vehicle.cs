using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovieFrontOffice.Models;

public class Vehicle
{
    public Vehicle(int id, string model, string make, int year, bool? availability, int typeId, int price, string image, DateTime? createdAt, DateTime? updatedAt)
    {
        Id = id;
        Model = model;
        Make = make;
        Year = year;
        Availability = availability;
        TypeId = typeId;
        Price = price;
        Image = image;
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
    public int Price { get; set; }

    [Required]
    public int TypeId { get; set; }
    
    [Required]
    public string Image { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}