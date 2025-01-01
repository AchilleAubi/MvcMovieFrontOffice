using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Controllers;

public class VehicleApiController(VehicleService vehicleService) : Controller
{
    private readonly VehicleService _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
    
    [HttpGet]
    [Route("api/vehicles")]
    public async Task<IActionResult> GetVehicles()
    {
        var vehicles = await _vehicleService.GetVehiclesAsync();
        return Json(vehicles); 
    }
    
    [HttpGet]
    [Route("api/vehicles/{id}")]
    public async Task<IActionResult> GetVehicleByIdApi([FromRoute] int id)
    {
        var vehicles = await _vehicleService.GetVehicleByIdAsync(id);
        return Json(vehicles); 
    }
    
    [HttpPost]
    [Route("api/vehicles")]
    public async Task<ActionResult> CreateVehicle([FromBody] Vehicle vehicle)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await vehicleService.CreateVehicleAsync(vehicle);
        return CreatedAtAction(nameof(GetVehicles), new { id = vehicle.Id }, vehicle);
    }
    
    [HttpPut("api/vehicles/{id}")]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] Vehicle vehicle)
    {
        if (id != vehicle.Id)
        {
            return BadRequest("ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await vehicleService.UpdateVehicleAsync(vehicle);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!vehicleService.VehicleExist(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("api/vehicles/{id}")]
    public async Task<IActionResult> DeleteVehicle(int id)
    {
        await vehicleService.DeleteVehicleAsync(id);
        return NoContent();
    }
    
    [HttpGet]
    [Route("api/vehicles/view")]
    public async Task<IActionResult> GetVehiclesViewApi([FromQuery] string? model = null, [FromQuery] string? make = null, [FromQuery] string? vehicleType = null, [FromQuery] bool? availability = null, [FromQuery] int offset = 0, [FromQuery] int limit = 6)
    {
        var vehicles = await _vehicleService.GetVehiclesViewAsync(offset, limit, model, make, vehicleType, availability);
        return Json(vehicles);
    }
    
    [HttpGet]
    [Route("api/vehicles/view/total")]
    public async Task<IActionResult> GetTotalVehicleViewCountAsync([FromQuery] string? make = null, [FromQuery] string? model = null, [FromQuery] string? vehicleType = null, [FromQuery] bool? availability = null)
    {
        int totalVehicleViewCountAsync = await _vehicleService.GetTotalVehicleViewCountAsync(make, model, vehicleType, availability);
        return Ok(totalVehicleViewCountAsync);
    }

    [HttpGet]
    [Route("api/vehicles/view/{id}")]
    public async Task<IActionResult> GetVehiclesViewByIdApi([FromRoute] int id)
    {
        var vehicle = await _vehicleService.GetVehicleViewByIdAsync(id);
        return Json(vehicle);
    }

    [HttpGet]
    [Route("api/vehicles/types")]
    public async Task<IActionResult> GetVehicleTypesApi()
    {
        var vehiclesType = await _vehicleService.GetVehicleTypesWithIdsAsync();
        return Json(vehiclesType);
    }
}