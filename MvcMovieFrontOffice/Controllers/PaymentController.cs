using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Controllers;

[Authorize]
public class PaymentController(PaymentService paymentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var payments = await paymentService.GetPaymentByUserIdAsync(GetCurrentUserId());
        return View(payments);
    }
    
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}