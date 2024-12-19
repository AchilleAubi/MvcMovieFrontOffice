using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Services;

public class VehicleService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<List<Vehicle>> GetVehiclesAsync()
    {
        var vehicles = await _context.Vehicles.ToListAsync();
        return vehicles; 
    }

    public async Task<List<VehicleView>> GetVehiclesViewAsync(int offset, int limit, string? model = null, string? make = null, string? vehicleType = null, bool? availability = null)
    {
        var query = _context.VehicleView.AsQueryable();

        if (!string.IsNullOrEmpty(make))
            query = query.Where(v => v.Make.Contains(make));

        if (!string.IsNullOrEmpty(model))
            query = query.Where(v => v.Model.Contains(model));

        if (!string.IsNullOrEmpty(vehicleType))
            query = query.Where(v => v.VehicleType == vehicleType);

        if (availability.HasValue)
            query = query.Where(v => v.Availability == availability.Value);

        return await query
            .OrderBy(v => v.VehicleId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> GetTotalVehicleViewCountAsync(string? model = null, string? make = null, string? vehicleType = null, bool? availability = null)
    {
        return await _context.VehicleView
            .Where(v => (string.IsNullOrEmpty(make) || v.Make.Contains(make)) &&
                        (string.IsNullOrEmpty(model) || v.Model.Contains(model)) &&
                        (string.IsNullOrEmpty(vehicleType) || v.VehicleType == vehicleType) &&
                        (!availability.HasValue || v.Availability == availability.Value))
            .CountAsync();
    }

    public async Task<VehicleView> GetVehicleViewByIdAsync(int id)
    {
        var vehicle = await _context.VehicleView
            .FirstOrDefaultAsync(v => v.VehicleId == id);
        return vehicle!;
    }
    
    public async Task<Vehicle> GetVehicleByIdAsync(int id)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id);
        return vehicle!;
    }

    public async Task<List<string>> GetVehicleTypesAsync()
    {
        return await _context.VehicleTypes
            .Select(vt => vt.Name)
            .ToListAsync();
    }

    public async Task<List<object>> GetVehicleTypesWithIdsAsync()
    {
        return await _context.VehicleTypes
            .Select(vt => new { vt.Id, vt.Name })
            .ToListAsync<object>();
    }
    
    public async Task CreateVehicleAsync(Vehicle vehicle)
    {
        _context.Add(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVehicleAsync(Vehicle vehicle)
    {
        _context.Update(vehicle);
        await _context.SaveChangesAsync();
    }
    
    public bool VehicleExist(int id)
    {
        return _context.Vehicles.Any(v => v.Id == id);
    }
}