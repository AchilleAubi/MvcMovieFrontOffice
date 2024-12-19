using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.Models;

public class VehicleView(
    int vehicleId,
    string model,
    string make,
    int year,
    bool availability,
    double price,
    DateTime createdAt,
    DateTime updatedAt,
    int vehicleTypeId,
    string vehicleType,
    int vehicleImageId,
    string vehicleImage)
{
    [Key]
    public int VehicleId { get; set; } = vehicleId;

    public string Model { get; set; } = model;
    public string Make { get; set; } = make;
    public int Year { get; set; } = year;
    public bool Availability { get; set; } = availability;

    public double Price { get; set; } = price;
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;

    public int VehicleTypeId { get; set; } = vehicleTypeId;
    public string VehicleType { get; set; } = vehicleType;

    public int VehicleImageId { get; set; } = vehicleImageId;
    public string VehicleImage { get; set; } = vehicleImage;
} 
