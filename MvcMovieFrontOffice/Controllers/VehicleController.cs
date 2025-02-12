using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Controllers;

public class VehicleController(VehicleService vehicleService) : Controller
{
    private readonly VehicleService _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));

        [HttpGet]
        public async Task<IActionResult> Index(string? model, string? make, string? vehicleType, bool? availability = null, DateTime? filterDate = null, int page = 1, int pageSize = 8)
        {
            ViewBag.SpecificList = await _vehicleService.GetSpecificVehiclesViewAsync(); 
            
            int offset = (page - 1) * pageSize;

            var vehicles = await _vehicleService.GetVehiclesViewAsync(offset, pageSize, model, make, vehicleType, availability, filterDate);
            int totalVehicles = await _vehicleService.GetTotalVehicleViewCountAsync(model, make, vehicleType, availability, filterDate);

            int totalPages = (int)Math.Ceiling(totalVehicles / (double)pageSize);

            ViewBag.VehicleTypes = await _vehicleService.GetVehicleTypesAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentMake = make;
            ViewBag.CurrentModel = model;
            ViewBag.CurrentVehicleType = vehicleType;
            ViewBag.CurrentAvailability = availability;
            ViewBag.CurrentFilterDate = filterDate;

            return View(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleService.GetVehicleViewByIdAsync(id);
            return View(vehicle);
        }
}