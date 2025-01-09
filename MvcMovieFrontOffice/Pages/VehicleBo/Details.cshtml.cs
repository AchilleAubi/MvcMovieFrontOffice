using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Pages.VehicleBo;

[Authorize(Roles = "Admin")]
public class Details : PageModel
{
    private readonly ApiService _apiService;
    
    public Details(ApiService apiService)
    {
        _apiService = apiService;
    }
    
    public VehicleView? VehicleView { get; private set; }
    
    public async Task OnGetAsync(int id)
    {
        var apiUrl = $"http://localhost:5259/api/vehicles/view/{id}";
            
        VehicleView = await _apiService.GetDataFromApiAsync<VehicleView>(apiUrl);
    }
}