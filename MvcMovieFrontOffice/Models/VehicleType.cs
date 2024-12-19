using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovieFrontOffice.Models;

[Table("vehicle_types")]
public class VehicleType
{
    public VehicleType(int id, string name)
    {
        Id = id;
        Name = name;
    }

    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } 
}