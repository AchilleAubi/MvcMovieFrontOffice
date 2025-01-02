using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Pages.Rapports;

public class Index : PageModel
{
    private readonly ApplicationDbContext _context;

    public Index(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public int RevenusTotaux { get; private set; }
    public List<VehicleOccupancy> VehicleOccupancies { get; private set; }

    
    public async Task<IActionResult> OnGetAsync()
    {
        RevenusTotaux = await _context.Reservations
            .Where(r => r.Status == "completed")
            .SumAsync(r => r.TotalPrice);

        VehicleOccupancies = await _context.Vehicles
            .Join(
                _context.Reservations.Where(r => r.Status == "completed"),
                v => v.Id,
                r => r.VehicleId,
                (v, r) => new { v, r }
            )
            .GroupBy(
                x => new { x.v.Id, x.v.Make, x.v.Model },
                x => x.r
            )
            .Select(g => new VehicleOccupancy
            {
                Id = g.Key.Id,
                Make = g.Key.Make,
                Model = g.Key.Model,
                OccupancyRate = (decimal)(
                    g.Sum(r => EF.Functions.DateDiffDay(r.StartDate, r.EndDate)) * 1.0
                    / EF.Functions.DateDiffDay(g.Min(r => r.StartDate), DateTime.Now) * 100
                )
            })
            .ToListAsync();
        
        return Page();
    }

}