using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Controllers;

[Authorize]
public class ReservationController(ReservationService reservationService, VehicleService vehicleService, InvoiceService invoiceService, PaymentService paymentService, WalletService walletService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var reservations = await reservationService.GetReservationsByIdUserAsync(GetCurrentUserId());
        return View(reservations);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservation = await reservationService.GetReservationByIdAsync(id.Value);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    public async Task<IActionResult> Create(int id)
    {
        var vehicle = await vehicleService.GetVehicleViewByIdAsync(id);
        var reservation = new ReservationViewModel
        {
            Reservation = new Reservation
            {
                VehicleId = id,
                UserId = GetCurrentUserId(),
                Status = "pending",
                TotalPrice = vehicle.Price,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            },
            VehicleView = vehicle
        };
        
        reservation.Reservation.TotalPrice = vehicle?.Price ?? 0;
        
        return View(reservation);
    }
    
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Reservation")] ReservationViewModel reservationViewModel)
    {
        if (!ModelState.IsValid)
        {
            var vehicle = await vehicleService.GetVehicleViewByIdAsync(reservationViewModel.Reservation.VehicleId);
            reservationViewModel.VehicleView = vehicle;
            // return View(reservationViewModel);
        }
        
        var userId = reservationViewModel.Reservation.UserId;
        var wallet = await walletService.GetWalletByIdAsync(userId);
        
        if (wallet.Amount < reservationViewModel.Reservation.Amount)
        {
            ViewBag.ErrorMessage = "Insufficient balance to make this payment.";
            return View(reservationViewModel);
        }
        
        var reservationId = await reservationService.CreateReservationAsync(reservationViewModel.Reservation);

        var payment = new Payment
        {
            ReservationId = reservationId,
            PaymentDate = DateTime.Now,
            Amount = reservationViewModel.Reservation.Amount,
            PaymentMethod = "Credit Card",
            UserId = reservationViewModel.Reservation.UserId
        };
        
        await paymentService.CreatePaymentAsync(payment);
        
        wallet.Amount -= reservationViewModel.Reservation.Amount;
        await walletService.UpdateWalletAsync(wallet);
        
        var invoiceService = new InvoiceService();
        var document = invoiceService.GetInvoicePdf(reservationViewModel.Reservation);
        
        MemoryStream stream = new MemoryStream();
        document.Save(stream);
        
        Response.ContentType = "application/pdf";
        Response.Headers["content-lenght"] = stream.Length.ToString();
        byte[] bytes = stream.ToArray();
        stream.Close();
        
        return File(bytes, "application/pdf", "reservation.pdf");
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservation = await reservationService.GetReservationByIdAsync(id.Value);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,UserId,StartDate,EndDate,Status,TotalPrice,CreatedAt,UpdatedAt,Amount", "PaymentAmount")] Reservation reservation, int? PaymentAmount)
    {
        if (id != reservation.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (PaymentAmount.HasValue && PaymentAmount.Value > 0)
                {
                    var payment = new Payment
                    {
                        ReservationId = reservation.Id,
                        PaymentDate = DateTime.Now,
                        Amount = PaymentAmount.Value,
                        PaymentMethod = "Credit Card",
                        UserId = reservation.UserId
                    };
                    

                    reservation.Amount += PaymentAmount.Value;
                    
                    var userId = reservation.UserId;
                    var wallet = await walletService.GetWalletByIdAsync(userId);
                    
                    wallet.Amount -= PaymentAmount.Value;

                    if (wallet.Amount < 0)
                    {
                        ViewBag.ErrorMessage = "Insufficient balance to make this payment.";
                        return View(reservation);
                    }

                    await paymentService.CreatePaymentAsync(payment);
                    await walletService.UpdateWalletAsync(wallet);
                }
                await reservationService.UpdateReservationAsync(reservation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!reservationService.ReservationExists(reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(reservation);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservation = await reservationService.GetReservationByIdAsync(id.Value);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await reservationService.DeleteReservationAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
