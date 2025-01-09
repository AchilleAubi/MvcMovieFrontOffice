using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Pages.VehicleBo;

[Authorize(Roles = "Admin")]
public class Delete(ApplicationDbContext context) : PageModel
{
    [BindProperty] public Vehicle Vehicle { get; set; } = default!;
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicle = await context.Vehicles.FirstOrDefaultAsync(m => m.Id == id);

        if (vehicle == null)
        {
            return NotFound();
        }
        else
        {
            Vehicle = vehicle;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicle = await context.Vehicles.FindAsync(id);
        if (vehicle != null)
        {
            Vehicle = vehicle;
            context.Vehicles.Remove(Vehicle);
            await context.SaveChangesAsync();
        }

        return RedirectToPage("/VehicleBo/Index");
    }
}