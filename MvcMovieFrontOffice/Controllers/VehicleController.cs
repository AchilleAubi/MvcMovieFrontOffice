using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Controllers;

public class VehicleController(VehicleService vehicleService) : Controller
{
    private readonly VehicleService _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));

    [HttpGet]
        public async Task<IActionResult> Index(string? model, string? make, string? vehicleType, bool? availability = null, int page = 1, int pageSize = 6)
        {
            int offset = (page - 1) * pageSize;

            var vehicles = await _vehicleService.GetVehiclesViewAsync(offset, pageSize, make, model, vehicleType, availability);
            int totalVehicles = await _vehicleService.GetTotalVehicleViewCountAsync(make, model, vehicleType, availability);

            int totalPages = (int)Math.Ceiling(totalVehicles / (double)pageSize);

            ViewBag.VehicleTypes = await _vehicleService.GetVehicleTypesAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentMake = make;
            ViewBag.CurrentModel = model;
            ViewBag.CurrentVehicleType = vehicleType;
            ViewBag.CurrentAvailability = availability;

            return View(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleService.GetVehicleViewByIdAsync(id);
            return View(vehicle);
        }
}