namespace MvcMovieFrontOffice.Models;

public class VehicleOccupancy
{
    public VehicleOccupancy(int id, string make, string model, decimal occupancyRate)
    {
        Id = id;
        Make = make;
        Model = model;
        OccupancyRate = occupancyRate;
    }

    public VehicleOccupancy()
    {
    }

    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public decimal OccupancyRate { get; set; }
}