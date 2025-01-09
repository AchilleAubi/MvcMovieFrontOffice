using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Controllers;

[Authorize]
public class WalletController(WalletService walletService) : Controller
{
    public async Task<IActionResult> Index()
    {
        Wallet wallet;

        try
        {
            wallet = await walletService.GetWalletByIdAsync(GetCurrentUserId());
        }
        catch (KeyNotFoundException)
        {
            wallet = new Wallet
            {
                Id = 0,
                Amount = 0,
                UserId = GetCurrentUserId()
            };
        }
        return View(wallet);
    }
   
    [HttpPost]
    public async Task<IActionResult> Submit(int amount)
    {
        var userId = GetCurrentUserId();

        try
        {
            var wallet = await walletService.GetWalletByIdAsync(userId);
            
            wallet.Amount = amount;
            await walletService.UpdateWalletAsync(wallet);
        }
        catch (KeyNotFoundException)
        {
            var newWallet = new Wallet
            {
                Amount = amount,
                UserId = userId
            };
            await walletService.CreateWalletAsync(newWallet);
        }

        return RedirectToAction("Index");
    }
    
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}