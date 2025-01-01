using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.Models;

public class VehicleImage
{
    public VehicleImage(int id, string imageUrl)
    {
        this.Id = id;
        this.ImageUrl = imageUrl;
    }

    public VehicleImage()
    {
    }

    [Key]
    public int Id { get; set; }
    
    public string ImageUrl { get; set; }
}