using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Pages.VehicleBo;

[Authorize(Roles = "Admin")]
public class Create : PageModel
{
    private readonly ApiService _apiService;

    public Create(ApiService apiService)
    {
        _apiService = apiService;
    }
    public List<VehicleType>? VehicleTypes { get; set; }
    
    public async Task OnGetAsync()
    {
        var typesUrl = "http://localhost:5259/api/vehicles/types";
        VehicleTypes = await _apiService.GetDataFromApiAsync<List<VehicleType>>(typesUrl);
    }
}