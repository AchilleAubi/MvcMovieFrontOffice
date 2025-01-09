using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Pages.VehicleBo;

[Authorize(Roles = "Admin")]
public class Edit(ApiService apiService, ApplicationDbContext context) : PageModel
{
    [BindProperty]
    public VehicleView? VehicleView { get; set; }
    public List<VehicleType>? VehicleTypes { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var apiUrl = $"http://localhost:5259/api/vehicles/view/{id}";
        VehicleView = await apiService.GetDataFromApiAsync<VehicleView>(apiUrl);

        if (VehicleView == null)
        {
            return RedirectToPage("/NotFound");
        }
        
        var typesUrl = "http://localhost:5259/api/vehicles/types";
        VehicleTypes = await apiService.GetDataFromApiAsync<List<VehicleType>>(typesUrl);

        return Page();
    }
}