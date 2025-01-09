using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Pages.VehicleBo;

[Authorize(Roles = "Admin")]
public class Index : PageModel
{
    private readonly ApiService _apiService;
    
    public Index(ApiService apiService)
    {
        _apiService = apiService;
    }
    
    public List<VehicleView>? VehicleViews { get; set; }
    public List<VehicleType>? VehicleTypes { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync(string? model, string? make, string? vehicleType, bool? availability = null,
        [FromQuery] int page = 1, int pageSize = 5)
    {
        CurrentPage = page;
        
        var offset = (page - 1) * pageSize;
        
        var apiUrl = $"http://localhost:5259/api/vehicles/view?offset={offset}&limit={pageSize}";
        if (!string.IsNullOrEmpty(model)) apiUrl += $"&model={model}";
        if (!string.IsNullOrEmpty(make)) apiUrl += $"&make={make}";
        if (!string.IsNullOrEmpty(vehicleType)) apiUrl += $"&vehicleType={vehicleType}";
        if (availability.HasValue) apiUrl += $"&availability={availability.Value.ToString().ToLower()}";
            
        VehicleViews = await _apiService.GetDataFromApiAsync<List<Models.VehicleView>>(apiUrl);
            
        var totalUrl = "http://localhost:5259/api/vehicles/view/total";
        var hasQuery = false;

        if (!string.IsNullOrEmpty(model))
        {
            totalUrl += (hasQuery ? "&" : "?") + $"model={model}";
            hasQuery = true;
        }

        if (!string.IsNullOrEmpty(make))
        {
            totalUrl += (hasQuery ? "&" : "?") + $"make={make}";
            hasQuery = true;
        }

        if (!string.IsNullOrEmpty(vehicleType))
        {
            totalUrl += (hasQuery ? "&" : "?") + $"vehicleType={vehicleType}";
            hasQuery = true;
        }

        if (availability.HasValue)
        {
            totalUrl += (hasQuery ? "&" : "?") + $"availability={availability.Value.ToString().ToLower()}";
        }

        var totalVehicles = await _apiService.GetDataFromApiAsync<int>(totalUrl);
        TotalPages = (int)Math.Ceiling(totalVehicles / (double)pageSize);
            
        var typesUrl = "http://localhost:5259/api/vehicles/types";
        VehicleTypes = await _apiService.GetDataFromApiAsync<List<VehicleType>>(typesUrl);
    }
}